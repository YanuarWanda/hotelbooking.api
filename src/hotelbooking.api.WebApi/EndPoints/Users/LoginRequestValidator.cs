using FluentValidation;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Username).Cascade(CascadeMode.Stop).NotNull().NotEmpty().EmailAddress();
        RuleFor(e => e.Password).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
    }
}