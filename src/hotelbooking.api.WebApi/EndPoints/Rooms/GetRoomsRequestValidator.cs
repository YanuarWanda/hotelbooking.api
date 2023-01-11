using FluentValidation;

namespace hotelbooking.api.WebApi.EndPoints.Rooms;

public class GetRoomsRequestValidator : AbstractValidator<GetRoomsRequest>
{
    public GetRoomsRequestValidator()
    {
        RuleForEach(e => e.FacilityIds).Cascade(CascadeMode.Stop).Must(IsGuid);
		RuleFor(e => e.CheckInDate).Cascade(CascadeMode.Stop).Must(BeAValidDate);
		RuleFor(e => e.CheckOutDate).Cascade(CascadeMode.Stop).Must(BeAValidDate);
    }

	private bool BeAValidDate(DateTime? date)
	{
		return !date.Equals(default(DateTime));
	}

	private bool IsGuid(string arg)
	{
		return Guid.TryParse(arg, out _);
	}
}