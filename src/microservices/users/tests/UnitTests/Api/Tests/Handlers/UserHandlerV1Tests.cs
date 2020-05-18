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
    using UnitTests.Common;

    public class UserHandlerV1Tests
    {
        [Fact]
        public async Task GetUserById_GivenTheUserExists_ShouldReturnCreatedUser()
        {
            //Arrange
            var user = new UserBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3")).Build();
            var getUserIdRequest = new GetUserByIdRequest { Uuid = user.Id.ToString() };
            var getUserUseCaseStub = A.Fake<IGetUserUseCase>();

            A.CallTo(() => getUserUseCaseStub.ExecuteAsync(user.Id))
                .Returns(user);

            var service = new UserHandlerV1(null, null, getUserUseCaseStub);

            //Act
            var response = await service.GetUserById(getUserIdRequest, TestServerCallContext.Create());

            //Assert
            response.User.Uuid.Should().NotBe(string.Empty);
            response.User.Name.Should().Be(user.Name);
            response.User.Email.Should().Be(user.Email);
            response.User.PhoneNumber.Should().Be(user.PhoneNumber);
        }

        [Fact]
        public void GetUserById_GivenTheUserDoesNotExist_ShouldThrowArgumentNullException()
        {
            //Arrange
            var userId = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3");
            var getUserIdRequest = new GetUserByIdRequest { Uuid = userId.ToString() };
            var getUserUseCaseStub = A.Fake<IGetUserUseCase>();

            A.CallTo(() => getUserUseCaseStub.ExecuteAsync(userId))
                .ThrowsAsync(() => new ArgumentNullException());

            var service = new UserHandlerV1(null, null, getUserUseCaseStub);

            //Act
            Func<Task> action = () => service.GetUserById(getUserIdRequest, TestServerCallContext.Create());

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

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

            var userHandler = new UserHandlerV1(null, createUserUseCaseStub, null);

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
