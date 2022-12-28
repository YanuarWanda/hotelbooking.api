using Microsoft.Extensions.Options;
using hotelbooking.api.SharedKernel;
using hotelbooking.api.WebApi;
using hotelbooking.api.WebApi.Services;

namespace hotelbooking.api.UnitTests;

public class JwtServiceBuilder
{
    private readonly JwtService _jwtService;

    public JwtServiceBuilder()
    {
        _jwtService =
            new JwtService(Options.Create(new ApplicationOption() {SecretKey = RandomHelper.GetSecureRandomString(64)}));
    }

    public JwtService Build()
    {
        return _jwtService;
    }
}