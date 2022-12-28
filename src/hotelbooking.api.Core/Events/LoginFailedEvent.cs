using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Events;

public class LoginFailedEvent : BaseDomainEvent
{
	public User User { get; }

	public LoginFailedEvent(User user)
	{
		User = user;
	}
}