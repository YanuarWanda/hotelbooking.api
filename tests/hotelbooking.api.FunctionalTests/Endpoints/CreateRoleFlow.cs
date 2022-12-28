using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi;
using hotelbooking.api.WebApi.EndPoints.RoleManagement;
using hotelbooking.api.WebApi.Models;
using Xunit;

namespace hotelbooking.api.FunctionalTests.Endpoints;

[Collection("Role API Collection")]
public class CreateRoleFlow : BaseEndpointTest
{
	private readonly HttpClient _client;
	private readonly IServiceProvider _services;

	public CreateRoleFlow(CustomWebApplicationFactory<WebMarker> factory) : base(factory)
	{
		_services = factory.Services;
		_client = factory.CreateClient();
	}

	/// <summary>
	/// PSEUDOCODE
	/// 1. Create ROLE
	/// 2. Get Role by ID
	/// 3. Result not null and exact value from create`s payload
	/// </summary>
	[Fact]
	public async Task CreateRoleFlowShouldBeOk()
	{
		var permissionService = _services.GetService<IPermissionService>()!;

		var permissionReadWriteUser = await permissionService.GetByNameAsync("users.readwrite", CancellationToken.None);

		var accessToken = await GetAccessTokenAsync();

		var createRoleRequest = new CreateRoleRequest();
		createRoleRequest.Name = "Observer";
		createRoleRequest.PermissionIds = new[] {permissionReadWriteUser!.PermissionId.ToString()};

		var req = HttpHelper.CreateRequest($"{_client.BaseAddress!.AbsoluteUri}{CreateRoleRequest.Route}",
			HttpMethod.Post,
			new AuthenticationHeaderValue("Bearer", accessToken),
			null,
			new StringContent(JsonHelper.Serialize(createRoleRequest), Encoding.UTF8, "application/json"));

		var result = await _client.SendAsync(req);
		var resp = await result.Content.ReadAsStringAsync();

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		var createRoleResponse = JsonHelper.Deserialize<KeyDto>(resp);

		req = HttpHelper.CreateRequest(
			$"{_client.BaseAddress!.AbsoluteUri}{GetRoleByIdRequest.BuildRoute(createRoleResponse.Id)}",
			HttpMethod.Get,
			new AuthenticationHeaderValue("Bearer", accessToken));
		result = await _client.SendAsync(req);

		resp = await result.Content.ReadAsStringAsync();

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		var roleResponse = JsonHelper.Deserialize<RoleResponse>(resp);

		roleResponse.name.Should().Be(createRoleRequest.Name);
	}
}