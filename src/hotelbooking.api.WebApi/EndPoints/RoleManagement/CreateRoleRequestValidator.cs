using FluentValidation;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.WebApi.EndPoints.RoleManagement;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
	private readonly IPermissionService _service;

	public CreateRoleRequestValidator(IPermissionService service)
	{
		_service = service;

		RuleFor(e => e.Name).Cascade(CascadeMode.Stop).NotNull().NotEmpty().MaximumLength(256);
		RuleFor(e => e.PermissionIds).Cascade(CascadeMode.Stop).NotNull().NotEmpty()
			.Must(e => e!.Length > 0)
			.Must(NotDuplicate);
		RuleForEach(e => e.PermissionIds).Cascade(CascadeMode.Stop).NotNull().Must(IsGuid)
			.MustAsync(PermissionExists);
	}

	private async Task<bool> PermissionExists(string arg1, CancellationToken arg2)
	{
		var permission = await _service.GetByIdAsync(arg1, arg2);

		return permission != null;
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

	private bool IsGuid(string arg)
	{
		return Guid.TryParse(arg, out _);
	}
}