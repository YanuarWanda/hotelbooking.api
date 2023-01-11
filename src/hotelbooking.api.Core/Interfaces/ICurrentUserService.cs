namespace hotelbooking.api.Core.Interfaces;

public interface ICurrentUserService
{
	/// <summary>
	/// Represent user id
	/// </summary>
	string? UserId { get; }

	void SetUserId(string userId);
}