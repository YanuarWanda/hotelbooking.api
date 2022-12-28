using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.WebApi;
using hotelbooking.api.WebApi.EndPoints.RoleManagement;
using Xunit;

namespace hotelbooking.api.UnitTests.Endpoints.RoleManagement;

public class CreateRoleTests
{
	public static readonly CreateRoleRequest TestCorrectCreateRoleRequest = new()
	{
		Name = "Role Test", PermissionIds = new[] {Guid.NewGuid().ToString()}
	};

	public static IEnumerable<object[]> TestCreateRoleRequestValidatorFailData =>
		new List<object[]>
		{
			new object[] {"empty init", new CreateRoleRequest()},
			new object[]
			{
				"name is null",
				new CreateRoleRequest {Name = null, PermissionIds = new[] {Guid.NewGuid().ToString()}}
			},
			new object[]
			{
				"name is empty 1",
				new CreateRoleRequest {Name = string.Empty, PermissionIds = new[] {Guid.NewGuid().ToString()}}
			},
			new object[]
			{
				"name is empty 2",
				new CreateRoleRequest {Name = "", PermissionIds = new[] {Guid.NewGuid().ToString()}}
			},
			new object[]
			{
				"Permission ids is null",
				new CreateRoleRequest {Name = "Super Administrator", PermissionIds = null}
			},
			new object[]
			{
				"Permission ids is empty",
				new CreateRoleRequest {Name = "Super Administrator", PermissionIds = Array.Empty<string>()}
			},
			new object[]
			{
				"Permission ids contain duplication",
				new CreateRoleRequest
				{
					Name = "Super Administrator",
					PermissionIds = new[]
					{
						Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
						new Guid("fc822f58-6c82-4d24-b04d-1f0fe3751a18").ToString(),
						new Guid("fc822f58-6c82-4d24-b04d-1f0fe3751a18").ToString()
					}
				}
			}
		};

	[Theory]
	[MemberData(nameof(TestCreateRoleRequestValidatorFailData))]
	public async Task CreateRoleRequestValidatorShouldFail(string type, CreateRoleRequest request)
	{
		var validator =
			new CreateRoleRequestValidator(new PermissionServiceBuilder().GetByIdAllParams().Build()
				.Object);

		var validationResult = await validator.ValidateAsync(request, CancellationToken.None);

		validationResult.IsValid.Should().BeFalse($"with type of {type}");
	}

	[Fact]
	public async Task CreateRoleRequestValidatorShouldReturnTrue()
	{
		var request = TestCorrectCreateRoleRequest;

		var fakeRoleReadRepoBuilder = new PermissionServiceBuilder();
		foreach (var item in request.PermissionIds!)
			fakeRoleReadRepoBuilder.GetByIdAsync(new Guid(item),
				new PermissionBuilder().WithDefaultValues().Id(new Guid(item)).Build());

		var validator = new CreateRoleRequestValidator(fakeRoleReadRepoBuilder.Build().Object);

		var validationResult = await validator.ValidateAsync(request, CancellationToken.None);

		validationResult.IsValid.Should().BeTrue();
	}

	[Theory]
	[MemberData(nameof(TestCreateRoleRequestValidatorFailData))]
	public async Task CreateRoleShouldReturnBadRequestWhenValidationFail(string note, CreateRoleRequest request)
	{
		var fakeReadRepoPermissionBuilder = new PermissionServiceBuilder();

		if (request.PermissionIds != null)
			foreach (var item in request.PermissionIds!)
				if (!string.IsNullOrWhiteSpace(item))
					fakeReadRepoPermissionBuilder.GetByIdAsync(new Guid(item),
						new PermissionBuilder().WithDefaultValues().Id(new Guid(item)).Build());

		var fakeReadRepoPermission = fakeReadRepoPermissionBuilder.Build();

		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		var fakeRoleServiceBuilder = new RoleServiceBuilder();
		var fakeRoleService = fakeRoleServiceBuilder.Build();

		var handler = new CreateRole(fakeReadRepoPermission.Object, fakeRoleService.Object,
			fakeApplicationDbContext.Object);

		var result = await handler.HandleAsync(request, CancellationToken.None);

		result.Should().BeOfType<BadRequestObjectResult>($"request note : {note}");

		var resultValue = (ObjectResult)result;

		resultValue.Value.Should().BeOfType<ErrorResponse>();
		var value = (ErrorResponse)resultValue.Value!;
		value.Payload.Should().NotBeNull();
	}

	[Fact]
	public void CreateRoleShouldReturnBadRequestWhenRequestNameAlreadyExists()
	{
		var request = TestCorrectCreateRoleRequest;

		var fakeReadRepoPermissionBuilder = new PermissionServiceBuilder();
		foreach (var item in request.PermissionIds!)
			fakeReadRepoPermissionBuilder.GetByIdAsync(new Guid(item),
				new PermissionBuilder().WithDefaultValues().Id(new Guid(item)).Build());
		var fakeReadRepoPermission = fakeReadRepoPermissionBuilder.Build();

		var fakeRoleServiceBuilder = new RoleServiceBuilder();
		//set is name exists with parameter request.Name!
		fakeRoleServiceBuilder.IsNameExists(request.Name!, true);
		var fakeRoleService = fakeRoleServiceBuilder.Build();

		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		var handler = new CreateRole(fakeReadRepoPermission.Object, fakeRoleService.Object,
			fakeApplicationDbContext.Object);

		var result = handler.HandleAsync(request, CancellationToken.None).GetAwaiter().GetResult();

		result.Should().BeOfType<BadRequestObjectResult>();

		var resultValue = (ObjectResult)result;

		resultValue.Value.Should().BeOfType<ErrorResponse>();
		var value = (ErrorResponse)resultValue.Value!;
		value.Message.Should().Be("Role name already exists");
	}

	[Fact]
	public void CreateRoleCorrectFlow()
	{
		var request = TestCorrectCreateRoleRequest;

		var fakeReadRepoPermissionBuilder = new PermissionServiceBuilder();
		foreach (var item in request.PermissionIds!)
			fakeReadRepoPermissionBuilder.GetByIdAsync(new Guid(item),
				new PermissionBuilder().WithDefaultValues().Id(new Guid(item)).Build());
		var fakeReadRepoPermission = fakeReadRepoPermissionBuilder.Build();

		var fakeRoleServiceBuilder = new RoleServiceBuilder();
		//set is name exists with parameter request.Name!
		fakeRoleServiceBuilder.IsNameExists(request.Name!, false);
		var fakeRoleService = fakeRoleServiceBuilder.Build();

		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		//check roles added to entity must correct from parameters
		fakeApplicationDbContext.Setup(e => e.Roles.Add(It.IsAny<Role>())).Callback<Role>(e =>
		{
			e.Name.Should().Be(request.Name);
			e.NormalizedName.Should().Be(request.Name!.ToUpperInvariant());
			e.RolePermissions.Should().HaveCount(x => x > 0);
			e.RolePermissions.Count.Should().Be(request.PermissionIds.Length);
		});

		var handler = new CreateRole(fakeReadRepoPermission.Object, fakeRoleService.Object,
			fakeApplicationDbContext.Object);

		var result = handler.HandleAsync(request, CancellationToken.None).GetAwaiter().GetResult();

		result.Should().BeOfType<OkObjectResult>();

		fakeApplicationDbContext.Verify(e => e.Roles.Add(It.IsAny<Role>()), Times.Once);
		fakeApplicationDbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}