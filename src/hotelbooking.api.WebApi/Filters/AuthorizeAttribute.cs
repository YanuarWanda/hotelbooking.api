using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Models;

namespace hotelbooking.api.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "ApiKey";

    public AuthorizeAttribute()
    {
    }

	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<ApplicationOption>>()
            .Value!;
	    var configApiKey = options.ApiKey!;

        if (!apiKey.Equals(configApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
	}
}