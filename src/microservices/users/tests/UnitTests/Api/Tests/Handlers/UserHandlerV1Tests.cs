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
            var userFake = new UserBuilder().Build();
            var getUserIdRequestStub = new GetUserByIdRequest { Uuid = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3").ToString() };

            ICreateUserUseCase createUserUseCaseStub = A.Fake<ICreateUserUseCase>();
            A.CallTo(() => createUserUseCaseStub.ExecuteAsync(
                A<CreateUserInput>.That.Matches(
                    i => i.Email == email
                    && i.Password == password
                    && i.Name == name
                    && i.PhoneNumber == phoneNumber)))
                .Returns(new User(id, email, password, name, phoneNumber));

            A.CallTo(() => this.stubbedGetUserUseCase.ExecuteAsync(A<Guid>.Ignored))
                .Returns(userFake);

            //Act
            var response = await serviceMock.GetUserById(getUserIdRequestStub, TestServerCallContext.Create());

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
            var getUserIdRequestStub = new GetUserByIdRequest { Uuid = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3").ToString() };

            var serviceMock = new UserHandlerV1(stubbedLogger, null, stubbedGetUserUseCase);

            A.CallTo(() => this.stubbedGetUserUseCase.ExecuteAsync(Guid.Empty))
                .ThrowsAsync(new ArgumentNullException());
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
