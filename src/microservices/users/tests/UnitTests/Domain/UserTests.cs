namespace UnitTests.Domain
{
    using FluentAssertions;
    using global::Domain;
    using global::Domain.Exceptions;
    using System;
    using Xunit;

    public class UserTests
    {
        [Fact]
        public void Initialize_WithValidData_ShouldCreateUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string email = "test@email.com";
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "08888888888";

            // Act
            Action act = () => new User(id, email, password, name, phoneNumber);

            // Assert
            act.Should().NotThrow();
        }

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
        public void Initialize_WithNullOrEmptyEmail_ShouldThrowDomainException(string email)
        {
            // Arrange
            var id = Guid.NewGuid();
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "0000";

            // Act
            Action act = () => new User(id, email, password, name, phoneNumber);

            // Assert
            act.Should().Throw<DomainException>()
                .And.Message.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test.email")]
        [InlineData("test@email")]
        [InlineData("test.email@com")]
        public void Initialize_WithInvalidEmailFormat_ShouldThrowDomainException(string email)
        {
            // Arrange
            var id = Guid.NewGuid();
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "0000";

            // Act
            Action act = () => new User(id, email, password, name, phoneNumber);

            // Assert
            act.Should().Throw<DomainException>()
                .And.Message.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Initialize_WithNullOrEmptyName_ShouldThrowDomainException(string name)
        {
            // Arrange
            Guid id = Guid.Empty;
            const string email = "test@email.com";
            const string password = "secret_password";
            const string phoneNumber = "0000";

            // Act
            Action act = () => new User(id, email, password, name, phoneNumber);

            // Assert
            act.Should().Throw<DomainException>()
                .And.Message.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("a")]
        public void Initialize_WithTooShortName_ShouldThrowDomainException(string name)
        {
            // Arrange
            Guid id = Guid.Empty;
            const string email = "test@email.com";
            const string password = "secret_password";
            const string phoneNumber = "0000";

            // Act
            Action act = () => new User(id, email, password, name, phoneNumber);

            // Assert
            act.Should().Throw<DomainException>()
                .And.Message.Should().NotBeNullOrEmpty();
        }
    }
}
