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
    using FakeItEasy;
    using UnitTests.Common;
    using global::Application.UseCases.Interfaces;

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
            var userFake = new UserBuilder().Build();
            var getUserIdRequestStub = new GetUserByIdRequest { Uuid = new Guid("4beabede-1b80-4663-9a21-97e41c2616d3").ToString() };

            var serviceMock = new UserHandlerV1(stubbedLogger, null, stubbedGetUserUseCase);

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

            //Act
            Func<Task> action = () => serviceMock.GetUserById(getUserIdRequestStub, TestServerCallContext.Create());

            //Assert
            action.Should().Throw<NullReferenceException>();
        }
    }
}
