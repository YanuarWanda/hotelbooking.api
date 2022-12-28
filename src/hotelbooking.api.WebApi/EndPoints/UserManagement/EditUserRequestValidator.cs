using FluentValidation;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class EditUserRequestValidator : AbstractValidator<EditUserRequest>
{
	private readonly IRoleService _roleService;

	public EditUserRequestValidator(IRoleService roleService)
	{
		_roleService = roleService;

		RuleFor(e => e.UserId).Cascade(CascadeMode.Stop).NotNull().NotEmpty().Must(IsGuid);
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

	private bool IsGuid(string? roleId)
	{
		return Guid.TryParse(roleId!, out _);
	}

	private async Task<bool> RoleExists(string roleId, CancellationToken cancellationToken)
	{
		var role = await _roleService.GetByIdAsync(roleId, cancellationToken);

		return role != null;
	}
}