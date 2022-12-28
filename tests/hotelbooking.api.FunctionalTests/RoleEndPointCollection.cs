using hotelbooking.api.WebApi;
using Xunit;

namespace hotelbooking.api.FunctionalTests;

[CollectionDefinition("Role API Collection")]
public class RoleEndPointCollection : ICollectionFixture<CustomWebApplicationFactory<WebMarker>>
{
}