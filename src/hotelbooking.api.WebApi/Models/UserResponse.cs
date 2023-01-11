namespace hotelbooking.api.WebApi.Models;

public record UserResponse(Guid userId, string? email, string? fullName);