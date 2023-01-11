using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Models;

namespace hotelbooking.api.WebApi;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApplicationOption _option;

    public JwtMiddleware(RequestDelegate next, IOptions<ApplicationOption> options)
    {
        _next = next;
        _option = options.Value;
    }

    public async Task Invoke(HttpContext context, ICurrentUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, userService, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, ICurrentUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_option.ApiKey!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userId = jwtToken.Claims.FirstOrDefault(e => e.Type == "id")!.Value;
            userService.SetUserId(userId);
        }
        catch
        {

        }
    }
}