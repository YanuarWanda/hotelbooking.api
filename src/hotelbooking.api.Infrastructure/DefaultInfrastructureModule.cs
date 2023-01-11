using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using hotelbooking.api.Core;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.Infrastructure.Data;
using hotelbooking.api.Infrastructure.Services;
using Module = Autofac.Module;

namespace hotelbooking.api.Infrastructure;

public class DefaultInfrastructureModule : Module
{
	private readonly bool _isDevelopment = false;
	private readonly List<Assembly> _assemblies = new List<Assembly>();

	public DefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
	{
		_isDevelopment = isDevelopment;
		var coreAssembly =
			Assembly.GetAssembly(typeof(CoreSeed)); // TODO: Replace "Project" with any type from your Core project
		var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));
		if (coreAssembly != null)
		{
			_assemblies.Add(coreAssembly);
		}

		if (infrastructureAssembly != null)
		{
			_assemblies.Add(infrastructureAssembly);
		}

		if (callingAssembly != null)
		{
			_assemblies.Add(callingAssembly);
		}
	}

	protected override void Load(ContainerBuilder builder)
	{
		if (_isDevelopment)
		{
			RegisterDevelopmentOnlyDependencies(builder);
		}
		else
		{
			RegisterProductionOnlyDependencies(builder);
		}

		RegisterCommonDependencies(builder);
	}

	private void RegisterCommonDependencies(ContainerBuilder builder)
	{
		builder
			.RegisterType<Mediator>()
			.As<IMediator>()
			.InstancePerLifetimeScope();

		builder.Register<ServiceFactory>(context =>
		{
			var c = context.Resolve<IComponentContext>();
			return t => c.Resolve(t);
		});

		var mediatrOpenTypes = new[]
		{
			typeof(IRequestHandler<,>), typeof(IRequestExceptionHandler<,,>), typeof(IRequestExceptionAction<,>),
			typeof(INotificationHandler<>),
		};

		foreach (var mediatrOpenType in mediatrOpenTypes)
		{
			builder
				.RegisterAssemblyTypes(_assemblies.ToArray())
				.AsClosedTypesOf(mediatrOpenType)
				.AsImplementedInterfaces();
		}

		builder.RegisterType<SmtpEmailSender>().As<IEmailSender>()
			.InstancePerLifetimeScope();

		builder.RegisterType<ApplicationDbContext>()
			.As<IApplicationDbContext>()
			.InstancePerLifetimeScope();

		builder.RegisterType<DateTimeService>()
			.As<IDateTime>()
			.InstancePerLifetimeScope();

		builder.RegisterType<UserService>()
			.As<IUserService>()
			.InstancePerLifetimeScope();
	}

	private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
	{
	}

	private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
	{
	}
}