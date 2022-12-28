using FluentValidation;
using hotelbooking.api.Core.Interfaces;

// ReSharper disable All

namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
	private readonly IRoleService _roleService;

	public CreateUserRequestValidator(IRoleService roleService)
	{
		_roleService = roleService;

		RuleFor(e => e.Username).Cascade(CascadeMode.Stop).NotNull().NotEmpty().EmailAddress();
		RuleFor(e => e.Password).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
		RuleFor(e => e.ConfirmPassword).Equal(e => e.Password);
		RuleFor(e => e.FirstName).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
		RuleFor(e => e.RoleIds).Cascade(CascadeMode.Stop).NotNull().Must(e => e!.Length > 0).Must(NotDuplicate);
		RuleForEach(e => e.RoleIds).Cascade(CascadeMode.Stop).NotNull().Must(IsGuid).MustAsync(RoleExists);
	}

	private bool NotDuplicate(string[]? arg)
	{
		if (arg == null || arg.Length == 0)
		{
			return false;
		}

		var length = arg!.Length;
		var lengthAfter = arg!.Distinct().Count();

		return length == lengthAfter;
	}

	private bool IsGuid(string roleId)
	{
		return Guid.TryParse(roleId, out _);
	}

	private async Task<bool> RoleExists(string roleId, CancellationToken cancellationToken)
	{
		var role = await _roleService.GetByIdAsync(roleId, cancellationToken);

		return role != null;
	}
}