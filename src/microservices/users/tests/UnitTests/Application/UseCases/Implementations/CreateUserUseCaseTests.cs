namespace UnitTests.Application.UseCases.Implementations
{
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;
    using System;
    using FakeItEasy;
    using global::Application.Infrastructure.Repositories;
    using global::Application.UseCases.Implementations;
    using global::Application.UseCases.Models;
    using global::Domain;

    public class CreateUserUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidInput_ShouldReturnCreatedUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            const string email = "test@email.com";
            const string password = "secret_password";
            const string name = "John Doe";
            const string phoneNumber = "08888888888";

            IUserRepository userRepositoryStub = A.Fake<IUserRepository>();
            A.CallTo(() => userRepositoryStub.CreateAsync(
                A<User>.That.Matches(
                    i => i.Email == email
                    && i.PasswordHash == password
                    && i.Name == name
                    && i.PhoneNumber == phoneNumber
                    && i.Id != Guid.Empty)))
                .Returns(new User(id, email, password, name, phoneNumber));

            var useCase = new CreateUserUseCase(userRepositoryStub);

            var input = new CreateUserInput(email, password, name, phoneNumber);

            // Act
            User actual = await useCase.ExecuteAsync(input);

            // Assert
            actual.Id.Should().Be(id);
            actual.Email.Should().Be(email);
            actual.PasswordHash.Should().Be(password);
            actual.Name.Should().Be(name);
            actual.PhoneNumber.Should().Be(phoneNumber);
        }
    }
}