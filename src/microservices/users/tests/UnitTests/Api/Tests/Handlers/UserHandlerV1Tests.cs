namespace UnitTests.Api.Tests.Handlers
{
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;
    using global::Api.Protos;
    using global::Api;
    using global::Application.UseCases.Interfaces;
    using global::Application.UseCases.Models;
    using System;
    using UnitTests.Api.Helpers;
    using FakeItEasy;
    using User = global::Domain.User;

    public class UserHandlerV1Tests
    {
        [Fact]
        public async Task CreateUser_WithValidParameters_ShouldReturnCreatedUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string email = "test@email.com";
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "08888888888";

            ICreateUserUseCase createUserUseCaseStub = A.Fake<ICreateUserUseCase>();
            A.CallTo(() => createUserUseCaseStub.ExecuteAsync(
                A<CreateUserInput>.That.Matches(
                    i => i.Email == email
                    && i.Password == password
                    && i.Name == name
                    && i.PhoneNumber == phoneNumber)))
                .Returns(new User(id, email, password, name, phoneNumber));

            var userHandler = new UserHandlerV1(null, createUserUseCaseStub);

            var request = new CreateUserRequest()
            {
                Email = email,
                Password = password,
                Name = name,
                PhoneNumber = phoneNumber
            };

            // Act
            CreateUserResponse actual = await userHandler.CreateUser(request, TestServerCallContext.Create());

            // Assert
            actual.Uuid.Should().Be(id.ToString());
            actual.Email.Should().Be(email);
            actual.Name.Should().Be(name);
            actual.PhoneNumber.Should().Be(phoneNumber);
        }
    }
}
