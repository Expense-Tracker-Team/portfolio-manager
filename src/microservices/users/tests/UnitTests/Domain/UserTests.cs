namespace UnitTests.Domain
{
    using FluentAssertions;
    using global::Domain;
    using System;
    using Xunit;

    public class UserTests
    {
        [Fact]
        public void Initialize_WithEmptyGuidId_ShouldGenerateNewId()
        {
            // Arrange
            Guid id = Guid.Empty;
            const string email = "test@email.com";
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "08888888888";

            // Act
            var actual = new User(id, email, password, name, phoneNumber);

            // Assert
            actual.Id.Should().NotBe(Guid.Empty);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Initialize_WithNullOrEmptyName_ShouldThrowArgumentNullException(string name)
        {
            // Arrange
            Guid id = Guid.Empty;
            const string email = "test@email.com";
            const string password = "secret_password";
            const string phoneNumber = "0000";

            // Act
            Action act = () => new User(id, email, password, name, phoneNumber);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().NotBeNullOrEmpty();
        }
    }
}
