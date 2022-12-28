using hotelbooking.api.WebApi;
using Xunit;

namespace hotelbooking.api.FunctionalTests;

[CollectionDefinition("User API Collection")]
public class UserEndPointCollection : ICollectionFixture<CustomWebApplicationFactory<WebMarker>>
{
}