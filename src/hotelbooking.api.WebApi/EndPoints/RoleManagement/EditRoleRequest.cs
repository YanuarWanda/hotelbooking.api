﻿using Microsoft.AspNetCore.Mvc;

namespace hotelbooking.api.WebApi.EndPoints.RoleManagement;

public class EditRoleRequest
{
    public const string Route = "api/role-management/roles/{RoleId}";
    public static string BuildRoute(Guid roleId) => Route.Replace("{RoleId}", roleId.ToString());

    [FromRoute(Name = "RoleId")] public string RoleId { get; set; } = default!;
    [FromBody] public EditRoleDto? Dto { get; set; }
}

public class EditRoleDto
{
    public string[]? PermissionIds { get; set; }
}