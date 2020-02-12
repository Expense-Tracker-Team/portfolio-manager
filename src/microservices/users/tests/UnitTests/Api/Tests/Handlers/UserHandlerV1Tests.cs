namespace UnitTests.Api.Tests.Handlers
{
    using System.Threading.Tasks;
    using Xunit;
    using Google.Protobuf.WellKnownTypes;
    using FluentAssertions;
    using Grpc.Core;
    using global::Api.Protos;
    using global::Api;
    using System;
    using UnitTests.Api.Helpers;

    public class UserHandlerV1Tests
    {
        [Fact]
        public async Task GetUsers_WhenUsersDoNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var service = new UserHandlerV1();

            // Act
            GetUsersResponse response = await service.GetUsers(new Empty(), TestServerCallContext.Create());

            // Asserts
            response.Users.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserById_WhenUserDoesNotExist_ShouldThrowRpcException()
        {
            // Arrange
            var service = new UserHandlerV1();

            string userId = Guid.NewGuid().ToString();
            var request = new GetUserByIdRequest
            {
                Uuid = userId
            };

            // Act
            RpcException thrownException = await Assert.ThrowsAsync<RpcException>(() => service.GetUserById(request, TestServerCallContext.Create()));

            // Assert
            thrownException.Status.StatusCode.Should().Be(StatusCode.NotFound);
            thrownException.Status.Detail.Should().Be($"User with id {userId} was not found");
        }
    }
}
