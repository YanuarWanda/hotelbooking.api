﻿namespace hotelbooking.api.WebApi.Models;

public record UserResponse(Guid userId, string? firstName, string? middleName, string? lastName, string? fullName);