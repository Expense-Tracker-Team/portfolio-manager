namespace UnitTests.Api.Tests.Handlers
{
    using System.Threading.Tasks;
    using Xunit;
    using Google.Protobuf.WellKnownTypes;
    using FluentAssertions;
    using Grpc.Core;
    using global::Api.Protos;
    using global::Api;
    using global::Application.UseCases.Interfaces;
    using global::Application.UseCases.Models;
    using System;
    using UnitTests.Api.Helpers;
    using Microsoft.Extensions.Logging;
    using Application.UseCases.Interfaces;
    using FakeItEasy;

    public class UserHandlerV1Tests
    {
        private readonly ILogger<UserHandlerV1> stubbedLogger;
        private readonly IGetUserUseCase stubbedGetUserUseCase;

        public UserHandlerV1Tests()
        {
            this.stubbedLogger = A.Fake<ILogger<UserHandlerV1>>();
            this.stubbedGetUserUseCase = A.Fake<IGetUserUseCase>();
        }

        [Fact]
        public async Task GetUserById_GivenTheUserExists_ShouldReturnUser()
        {
            //Arrange
            var stubbedUser = new UserBuilder().Build();
            var getUserIdRequestStub = new GetUserByIdRequest { Uuid = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3").ToString() };

            var serviceMock = new UserHandlerV1(stubbedLogger, null, stubbedGetUserUseCase);

            A.CallTo(() => this.stubbedGetUserUseCase.ExecuteAsync(A<Guid>.Ignored))
                .Returns(stubbedUser);

            //Act
            var response = await serviceMock.GetUserById(getUserIdRequestStub, TestServerCallContext.Create());

            //Assert
            response.User.Uuid.Should().NotBe(string.Empty);
            response.User.Name.Should().Be(stubbedUser.Name);
            response.User.Email.Should().Be(stubbedUser.Email);
            response.User.Password.Should().Be(stubbedUser.PasswordHash);
            response.User.PhoneNumber.Should().Be(stubbedUser.PhoneNumber);
        }

        [Fact]
        public async Task GetUserById_GivenTheUserDoesNotExist_ShouldThrowArgumentNullException()
        {
            //Arrange
            var getUserIdRequestStub = new GetUserByIdRequest { Uuid = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3").ToString() };

            var serviceMock = new UserHandlerV1(stubbedLogger, null, stubbedGetUserUseCase);

            A.CallTo(() => this.stubbedGetUserUseCase.ExecuteAsync(A<Guid>.Ignored))
                .ThrowsAsync(new ArgumentNullException());

            //Act
            Func<Task> action = () => serviceMock.GetUserById(getUserIdRequestStub, TestServerCallContext.Create());

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
