using FluentValidation;

namespace hotelbooking.api.WebApi.EndPoints.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
		RuleFor(e => e.RoomId).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
		RuleFor(e => e.CheckInDate).Cascade(CascadeMode.Stop).Must(BeAValidDate).LessThan(e => e.CheckOutDate);
		RuleFor(e => e.CheckOutDate).Cascade(CascadeMode.Stop).Must(BeAValidDate).GreaterThan(e => e.CheckInDate);
    }

	private bool BeAValidDate(DateTime date)
	{
		return !date.Equals(default(DateTime));
	}
}