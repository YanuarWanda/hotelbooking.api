namespace hotelbooking.api.WebApi.Models;

public class ApplicationOption
{
	private const int DefaultExpireDuration = 86400;

	public string? ApiKey { get; set; }

	public string? DoMigration { get; set; }

	public bool GetDoMigration()
	{
		try
		{
			if (DoMigration != null)
				return bool.Parse(DoMigration);
		}
		catch
		{
			// ignored
		}

		return false;
	}
}