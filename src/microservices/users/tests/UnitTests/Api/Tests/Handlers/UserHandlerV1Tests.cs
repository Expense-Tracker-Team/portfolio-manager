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
        [Fact]
        public async Task GetUserById_GivenTheUserExists_ShouldReturnUser()
        {
            //Arrange
            var userFake = new UserBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3")).Build();
            var getUserIdRequest = new GetUserByIdRequest { Uuid = userFake.Id.ToString() };
            var stubbedGetUserUseCase = A.Fake<IGetUserUseCase>();
            var stubbedLogger = A.Fake<ILogger<UserHandlerV1>>();

            A.CallTo(() => stubbedGetUserUseCase.ExecuteAsync(userFake.Id))
                .Returns(userFake);

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

            var service = new UserHandlerV1(stubbedLogger, null, stubbedGetUserUseCase);

            //Act
            Func<Task> action = () => service.GetUserById(getUserIdRequest, TestServerCallContext.Create());

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
