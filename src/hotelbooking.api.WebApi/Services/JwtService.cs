using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.WebApi.Models;

namespace hotelbooking.api.WebApi.Services;

public class JwtService
{
    private readonly ApplicationOption _option;

    public JwtService(IOptions<ApplicationOption> options)
    {
        _option = options.Value;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim("id", user.UserId.ToString()));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_option.ApiKey!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}