using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Events;

public class UserRegisteredEvent : BaseDomainEvent
{
	public User User { get; }

	public UserRegisteredEvent(User user)
	{
		User = user;
	}
}