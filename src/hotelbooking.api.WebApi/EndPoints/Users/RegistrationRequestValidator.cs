using FluentValidation;
using hotelbooking.api.Core.Interfaces;

// ReSharper disable All

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
	public RegistrationRequestValidator()
	{
		RuleFor(e => e.Email).Cascade(CascadeMode.Stop).NotNull().NotEmpty().EmailAddress();
		RuleFor(e => e.Password).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
		RuleFor(e => e.ConfirmPassword).Equal(e => e.Password);
		RuleFor(e => e.Name).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
	}
}