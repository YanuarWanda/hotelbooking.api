using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Events;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Filters;
using hotelbooking.api.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;

namespace hotelbooking.api.WebApi.EndPoints.Bookings;

public class CreateBooking : EndpointBaseAsync.WithRequest<CreateBookingRequest>.WithActionResult<CreateBookingResponse>
{
	private readonly IBookingService _bookingService;
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public CreateBooking(IBookingService bookingService, IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_bookingService = bookingService;
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	[Authorize(isLogin: true)]
	[HttpPost(CreateBookingRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(CreateBookingResponse))]
	[SwaggerOperation(
		Summary = "API Create Booking",
		OperationId = "Bookings.Post",
		Tags = new[] {"Bookings"})
	]
	public override async Task<ActionResult<CreateBookingResponse>> HandleAsync([FromBody] CreateBookingRequest request, CancellationToken cancellationToken = new())
	{
		var validator = new CreateBookingRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));
		}

		var isRoomBooked = await _bookingService.IsRoomBooked(request.RoomId!, cancellationToken);
		if (isRoomBooked)
			return BadRequest(ErrorResponseExtension.Create("Room already booked"));

		var booking = new Booking();
		booking.UserId = new Guid(_currentUserService.UserId!);
		booking.RoomId = request.RoomId;
		booking.CheckInDate = request.CheckInDate;
		booking.CheckOutDate = request.CheckOutDate;

		booking.Events.Add(new BookingSuccessEvent(booking));
		await _dbContext.Bookings.AddAsync(booking);
		await _dbContext.SaveChangesAsync(cancellationToken);

		booking = _dbContext.Bookings.Include(x => x.Room).FirstOrDefault(x => x.BookingId.Equals(booking.BookingId));

		System.Console.WriteLine(JsonConvert.SerializeObject(booking));

		return Ok(CreateBookingResponseExtension.Build(booking!));
	}
}