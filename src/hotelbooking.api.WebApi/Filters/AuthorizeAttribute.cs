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
	private bool _isLogin { get; }

    public AuthorizeAttribute(bool isLogin = false)
    {
		_isLogin = isLogin;
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

		if (_isLogin)
		{
			var service = context.HttpContext.RequestServices.GetService<ICurrentUserService>()!;

			if (service.UserId == null)
			{
				context.Result =
					new JsonResult(new ErrorResponse {Message = "Unauthorized"})
					{
						StatusCode = StatusCodes.Status401Unauthorized
					};
				return;
			}
		}

        await next();
	}
}