using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Events;
using hotelbooking.api.WebApi;
using hotelbooking.api.WebApi.EndPoints.UserManagement;
using hotelbooking.api.WebApi.Models;
using Xunit;

namespace hotelbooking.api.UnitTests.Endpoints.UserManagement;

public class CreateUserTests
{
	public static readonly CreateUserRequest TestCorrectCreateUserRequest = new CreateUserRequest
	{
		Username = "test@mail.com",
		Password = "Qwerty@123",
		ConfirmPassword = "Qwerty@123",
		FirstName = "Test",
		LastName = "User",
		RoleIds = new[] {Guid.NewGuid().ToString()}
	};

	public static IEnumerable<object[]> TestCreateUserValidatorFailData =>
		new List<object[]>
		{
			//empty init
			new object[] {"empty init", new CreateUserRequest()},
			//User null
			new object[]
			{
				"User null",
				new CreateUserRequest()
				{
					Username = null,
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = "Lorep Ipsum",
					RoleIds = new[] {Guid.NewGuid().ToString()}
				}
			},
			//user not email
			new object[]
			{
				"User not email format",
				new CreateUserRequest()
				{
					Username = "test",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = "Lorep Ipsum",
					RoleIds = new[] {Guid.NewGuid().ToString()}
				}
			},
			//confirm password not same
			new object[]
			{
				"Confirm password not same as pasword property",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Hehehehe",
					FirstName = "Lorep Ipsum",
					RoleIds = new[] {Guid.NewGuid().ToString()}
				}
			},
			//First name is null
			new object[]
			{
				"First name is null",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = null,
					RoleIds = new[] {Guid.NewGuid().ToString()}
				}
			},
			//First name is empty
			new object[]
			{
				"First name is empty",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = string.Empty,
					RoleIds = new[] {Guid.NewGuid().ToString()}
				}
			},
			//First name is space only
			new object[]
			{
				"First name is space only",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = "   ",
					RoleIds = new[] {Guid.NewGuid().ToString()}
				}
			},
			//RoleIds null
			new object[]
			{
				"RoleIds is null",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Hehehehe",
					FirstName = "Lorep Ipsum",
					RoleIds = null
				}
			},
			//RoleIds empty
			new object[]
			{
				"RoleIds is empty",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = "Lorep Ipsum",
					RoleIds = Array.Empty<string>()
				}
			},
			//RoleIds exist but contain one of non-guid
			new object[]
			{
				"RoleIds exists but contain one or more non-guid",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = "Lorep Ipsum",
					RoleIds = new[] {Guid.NewGuid().ToString(), "hehe", "   "}
				}
			},
			//RoleIds exist but contain duplication
			new object[]
			{
				"RoleIds exists but contain duplication",
				new CreateUserRequest()
				{
					Username = "test@mail.com",
					Password = "Qwerty@123",
					ConfirmPassword = "Qwerty@123",
					FirstName = "Lorep Ipsum",
					RoleIds = new[]
					{
						Guid.NewGuid().ToString(), "683c417d-775a-428d-b958-e62871c70fc7",
						"683c417d-775a-428d-b958-e62871c70fc7", Guid.NewGuid().ToString(),
						"4b32bd8d-ea43-4e74-96dd-274e423e59ea", "4b32bd8d-ea43-4e74-96dd-274e423e59ea"
					}
				}
			}
		};

	[Theory]
	[MemberData(nameof(TestCreateUserValidatorFailData))]
	public async Task CreateUserRequestValidatorShouldReturnFalse(string type, CreateUserRequest request)
	{
		var validator =
			new CreateUserRequestValidator(new RoleServiceBuilder().GetByIdAllParams().Build()
				.Object);

		var validationResult = await validator.ValidateAsync(request, CancellationToken.None);

		validationResult.IsValid.Should().BeFalse($"with type of {type}");
	}

	[Fact]
	public async Task CreateUserRequestValidatorShouldReturnTrue()
	{
		var request = TestCorrectCreateUserRequest;

		var fakeRoleReadRepoBuilder = new RoleServiceBuilder();
		foreach (var item in request.RoleIds!)
			fakeRoleReadRepoBuilder.GetByIdAsync(new Guid(item),
				new RoleBuilder().WithDefaultValues().Id(new Guid(item)).Build());

		var validator = new CreateUserRequestValidator(fakeRoleReadRepoBuilder.Build().Object);

		var validationResult = await validator.ValidateAsync(request, CancellationToken.None);

		validationResult.IsValid.Should().BeTrue();
	}

	[Theory]
	[MemberData(nameof(TestCreateUserValidatorFailData))]
	public async Task CreateUserShouldReturnBadRequestWhenValidationIsFalse(string type, CreateUserRequest request)
	{
		var fakeRoleRepository = new RoleServiceBuilder();
		fakeRoleRepository.GetByIdAllParams();

		var fakeUserService = new UserServiceBuilder();

		var fakeApplicationDbContext = new InterfaceApplicationDbContextBuilder();

		var create = new CreateUser(fakeRoleRepository.Build().Object, fakeUserService.Build().Object,
			fakeApplicationDbContext.Build().Object);

		var result = await create.HandleAsync(request, CancellationToken.None);
		result.Should().BeOfType<BadRequestObjectResult>(because: $"request type {type}");

		var resultValue = (ObjectResult)result;
		resultValue.Value.Should().BeOfType<ErrorResponse>();

		var value = (ErrorResponse)resultValue.Value!;
		value.Payload.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateUserShouldReturnNotFoundWhenUsernameAlreadyExists()
	{
		var request = TestCorrectCreateUserRequest;

		var fakeRoleRepository = new RoleServiceBuilder();
		foreach (var item in request.RoleIds!)
			fakeRoleRepository.GetByIdAsync(new Guid(item),
				new RoleBuilder().WithDefaultValues().Id(new Guid(item)).Build());

		var fakeUserService = new UserServiceBuilder();
		fakeUserService.SetupGetUserByUsername(request.Username,
			new UserBuilder().WithDefaultValues().Username(request.Username!).Build());

		var fakeApplicationDbContext = new InterfaceApplicationDbContextBuilder();

		var create = new CreateUser(fakeRoleRepository.Build().Object, fakeUserService.Build().Object,
			fakeApplicationDbContext.Build().Object);

		var result = await create.HandleAsync(request, CancellationToken.None);

		result.Should().BeOfType<BadRequestObjectResult>();
		var resultValue = (ObjectResult)result;

		resultValue.Value.Should().BeOfType<ErrorResponse>();

		var value = (ErrorResponse)resultValue.Value!;
		value.Message.Should().Be("Username already exists");
		value.Payload.Should().BeNull();
	}

	[Fact]
	public async Task CreateUserShouldReturnNoContentResult()
	{
		var request = TestCorrectCreateUserRequest;

		var faceReadRepositoryBuilder = new RoleServiceBuilder();
		foreach (var item in request.RoleIds!)
			faceReadRepositoryBuilder.GetByIdAsync(new Guid(item),
				new RoleBuilder().WithDefaultValues().Id(new Guid(item)).Build());
		var fakeReadRepository = faceReadRepositoryBuilder.Build();

		var fakeUserServiceBuilder = new UserServiceBuilder();
		var fakeUserService = fakeUserServiceBuilder.Build();

		var callBackUserId = Guid.NewGuid();
		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		fakeApplicationDbContextBuilder.SetupUser(new List<User>());
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();
		fakeApplicationDbContext.Setup(e => e.Users.Add(It.IsAny<User>()))
			.Callback<User>((data) =>
			{
				callBackUserId = data.UserId;
				data.UserPasswords.Should().NotBeEmpty();
				data.UserRoles.Should().NotBeEmpty();
				data.Events.Should().NotBeEmpty();
				data.Events.Count.Should().Be(1);
				data.Events.First().Should().BeOfType<UserRegisteredEvent>();
			});

		var create = new CreateUser(fakeReadRepository.Object, fakeUserService.Object,
			fakeApplicationDbContext.Object);

		var result = await create.HandleAsync(request, CancellationToken.None);

		result.Should().BeOfType<OkObjectResult>();

		var resultValue = (ObjectResult)result;
		var value = (KeyDto)resultValue.Value!;

		value.Id.Should().Be(callBackUserId.ToString());
		fakeApplicationDbContext.Verify(e => e.Users.Add(It.IsAny<User>()), Times.Once);
		fakeApplicationDbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}