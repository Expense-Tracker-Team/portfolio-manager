namespace FunctionalTests.Api.Tests.Handlers
{
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;
    using FunctionalTests.Api.Base;
    using global::Api.Protos;
    using System;

    public class UserHandlerV1Tests : FunctionalTestBase
    {
        [Fact]
        public async Task CreateUser_WithValidUser_ShouldReturnCreatedUser()
        {
            // Arrange
            const string email = "test@email.com";
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "0888888888";

            var client = new Users.UsersClient(this.Channel);
            var request = new CreateUserRequest
            {
                Email = email,
                Password = password,
                Name = name,
                PhoneNumber = phoneNumber
            };

            // Act
            CreateUserResponse actual = await client.CreateUserAsync(request);

            // Assert
            actual.Uuid.Should().NotBe(Guid.Empty.ToString());
            actual.Email.Should().Be(email);
            actual.Name.Should().Be(name);
            actual.PhoneNumber.Should().Be(phoneNumber);
        }

        [Fact]
        public async Task GetUser_GivenValidId_ShouldReturnUser()
        {
            // Arrange
            const string email = "test@email.com";
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "0888888888";

            var client = new Users.UsersClient(this.Channel);

            var createUserRequest = new CreateUserRequest
            {
                Email = email,
                Password = password,
                Name = name,
                PhoneNumber = phoneNumber
            };

            CreateUserResponse createUserResponse = await client.CreateUserAsync(createUserRequest);

            var getUserRequest = new GetUserByIdRequest
            {
                Uuid = createUserResponse.Uuid
            };

            // Act
            GetUserByIdResponse response = await client.GetUserByIdAsync(getUserRequest);

            // Assert
            response.User.Uuid.Should().NotBe(Guid.Empty.ToString());
            response.User.Uuid.Should().Be(createUserResponse.Uuid);
            response.User.Email.Should().Be(email);
            response.User.Name.Should().Be(name);
            response.User.PhoneNumber.Should().Be(phoneNumber);
        }
    }
}
