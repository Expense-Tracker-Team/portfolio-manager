namespace FunctionalTests.Api.Tests.Handlers
{
    using System.Threading.Tasks;
    using Xunit;
    using Google.Protobuf.WellKnownTypes;
    using FluentAssertions;
    using FunctionalTests.Api.Base;
    using global::Api.Protos;

    public class UserHandlerV1Tests : FunctionalTestBase
    {
        [Fact]
        public async Task GetUsers_ReturnsAllUsers()
        {
            // Arrange
            var client = new Users.UsersClient(this.Channel);

            // Act
            GetUsersResponse response = await client.GetUsersAsync(new Empty());

            // Assert
            response.Users.Count.Should().Be(0);
        }
    }
}
