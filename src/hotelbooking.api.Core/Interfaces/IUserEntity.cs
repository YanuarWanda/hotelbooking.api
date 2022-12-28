using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Core.Interfaces;

public interface IUserEntity
{
	DbSet<User> Users { get; }
	DbSet<UserLogin> UserLogins { get; }
	DbSet<UserRole> UserRoles { get; }
	DbSet<UserPassword> UserPasswords { get; }
	DbSet<UserToken> UserTokens { get; }
	DbSet<Role> Roles { get; }
	DbSet<Permission> Permissions { get; }
	DbSet<RolePermission> RolePermissions { get; }
}