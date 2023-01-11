namespace hotelbooking.api.WebApi.EndPoints.Rooms;

public class GetRoomsRequest
{
    public const string Route = "api/rooms";

    public string[]? FacilityIds { get; set; }
}