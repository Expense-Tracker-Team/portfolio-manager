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
    using Microsoft.Extensions.Logging;
    using FakeItEasy;
    using User = global::Domain.User;
    using UnitTests.Common;
    using global::Application.UseCases.Interfaces;

    public class UserHandlerV1Tests
    {
        [Fact]
        public async Task CreateUser_WithValidParameters_ShouldReturnCreatedUser()
        {
            //Arrange
            var userFake = new UserBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3")).Build();
            var getUserIdRequest = new GetUserByIdRequest { Uuid = userFake.Id.ToString() };
            var stubbedGetUserUseCase = A.Fake<IGetUserUseCase>();
            var stubbedLogger = A.Fake<ILogger<UserHandlerV1>>();

            ICreateUserUseCase createUserUseCaseStub = A.Fake<ICreateUserUseCase>();
            A.CallTo(() => createUserUseCaseStub.ExecuteAsync(
                A<CreateUserInput>.That.Matches(
                    i => i.Email == email
                    && i.Password == password
                    && i.Name == name
                    && i.PhoneNumber == phoneNumber)))
                .Returns(new User(id, email, password, name, phoneNumber));

            var service = new UserHandlerV1(stubbedLogger, null, stubbedGetUserUseCase);

            //Act
            var response = await service.GetUserById(getUserIdRequest, TestServerCallContext.Create());

            //Assert
            response.User.Uuid.Should().NotBe(string.Empty);
            response.User.Name.Should().Be(userFake.Name);
            response.User.Email.Should().Be(userFake.Email);
            response.User.Password.Should().Be(userFake.PasswordHash);
            response.User.PhoneNumber.Should().Be(userFake.PhoneNumber);
        }

        [Fact]
        public async Task GetUserById_GivenTheUserDoesNotExist_ShouldThrowArgumentNullException()
        {
            //Arrange
            var userId = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3");
            var getUserIdRequest = new GetUserByIdRequest { Uuid = userId.ToString() };
            var stubbedGetUserUseCase = A.Fake<IGetUserUseCase>();
            var stubbedLogger = A.Fake<ILogger<UserHandlerV1>>();

            A.CallTo(() => stubbedGetUserUseCase.ExecuteAsync(userId))
                .ThrowsAsync(() => new ArgumentNullException());

            A.CallTo(() => this.stubbedGetUserUseCase.ExecuteAsync(Guid.Empty))
                .ThrowsAsync(new ArgumentNullException());
            var request = new CreateUserRequest()
            {
                Email = email,
                Password = password,
                Name = name,
                PhoneNumber = phoneNumber
            };

            //Act
            Func<Task> action = () => service.GetUserById(getUserIdRequest, TestServerCallContext.Create());

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
