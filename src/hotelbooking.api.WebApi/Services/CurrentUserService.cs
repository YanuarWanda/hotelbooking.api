using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId { get; private set; }

    public void SetUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(UserId))
            UserId = userId;
    }
}