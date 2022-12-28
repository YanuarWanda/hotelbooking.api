﻿using Microsoft.AspNetCore.Mvc;

namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class DeleteUserRequest
{
	public const string Route = "api/user-management/users/{UserId}";
	public static string BuildRoute(Guid userId) => Route.Replace("{UserId}", userId.ToString());
	public static string BuildRoute(string userId) => Route.Replace("{UserId}", userId);

	[FromRoute(Name = "UserId")] public string UserId { get; set; } = default!;
}