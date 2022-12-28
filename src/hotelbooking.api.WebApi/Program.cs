using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using hotelbooking.api.Core;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.Infrastructure;
using hotelbooking.api.Infrastructure.Data;
using hotelbooking.api.WebApi;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.
builder.Services.Configure<ApplicationOption>(builder.Configuration.GetSection("ApplicationOptions"));

builder.Services.AddDefaultDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers(opt =>
{
	opt.Filters.Add<ApiExceptionFilterAttribute>();
}).AddJsonOptions(opts =>
{
	opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
	opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.OperationFilter<AddAuthorizationHeaderOperationFilter>();

	c.SwaggerDoc(
		name: "v1",
		info: new OpenApiInfo {Title = "My API", Version = "v1"});

	c.AddSecurityDefinition(
		name: "Bearer",
		securityScheme: new OpenApiSecurityScheme
		{
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
		});

	c.EnableAnnotations();
});
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});
builder.Services.Configure<RouteOptions>(options =>
{
	options.LowercaseUrls = true;
});
builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});
builder.Services.AddCorsConfiguration();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
	containerBuilder.RegisterModule(
		new DefaultInfrastructureModule(builder.Environment.IsDevelopment()));
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
	config.Services = new List<ServiceDescriptor>(builder.Services);

	config.Path = "/registered-services";
});

builder.Logging.ClearProviders();

if (builder.Environment.IsDevelopment())
{
	builder.Logging.AddDebug();
}
else
	builder.Logging.AddSerilog(new LoggerConfiguration().ReadFrom
		.Configuration(builder.Configuration, sectionName: "SerilogOptions")
		.CreateLogger());

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
	app.UseShowAllServicesMiddleware();
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

// Enable middleware for jwt authorization
app.UseMiddleware<JwtMiddleware>();

app.UseEndpoints(endpoints =>
{
	endpoints.MapDefaultControllerRoute();
});

using (var scope = app.Services.CreateScope())
{
	var options = scope.ServiceProvider.GetRequiredService<IOptions<ApplicationOption>>();
	var option = options.Value!;

	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

	if (option.GetDoMigration())
		await context.Database.MigrateAsync();

	await CoreSeed.Seeder(context);
}

app.Run();