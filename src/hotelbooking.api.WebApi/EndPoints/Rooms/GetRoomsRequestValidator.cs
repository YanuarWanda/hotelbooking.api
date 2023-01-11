using FluentValidation;

namespace hotelbooking.api.WebApi.EndPoints.Rooms;

public class GetRoomsRequestValidator : AbstractValidator<GetRoomsRequest>
{
    public GetRoomsRequestValidator()
    {
        RuleForEach(e => e.FacilityIds).Cascade(CascadeMode.Stop).Must(IsGuid);
    }

	private bool IsGuid(string arg)
	{
		return Guid.TryParse(arg, out _);
	}
}