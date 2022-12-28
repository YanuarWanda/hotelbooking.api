using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Events;

public class LoginSuccessEvent : BaseDomainEvent
{
	public User User { get; }

	public LoginSuccessEvent(User user)
	{
		User = user;
	}
}