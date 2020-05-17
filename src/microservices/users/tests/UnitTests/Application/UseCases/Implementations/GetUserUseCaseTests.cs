namespace UnitTests.Application.UseCases.Implementations
{
    using FakeItEasy;
    using FluentAssertions;
    using global::Application.Infrastructure.Repositories;
    using global::Application.UseCases.Implementations;
    using System;
    using System.Threading.Tasks;
    using UnitTests.Common;
    using Xunit;

    public class GetUserUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_GivenUserId_ShouldReturnUser()
        {
            //Arrange
            var userFake = new UserBuilder().Build();
            var userRepositoryFake = A.Fake<IUserRepository>();

            A.CallTo(() => userRepositoryFake.Get(A<Guid>.Ignored))
                .Returns(userFake);

            var getUserUseCase = new GetUserUseCase(userRepositoryFake);

            //Act
            var response = await getUserUseCase.ExecuteAsync(Guid.NewGuid());

            //Assert
            response.Id.Should().NotBe(Guid.Empty);
            response.Name.Should().Be(userFake.Name);
            response.Email.Should().Be(userFake.Email);
            response.PasswordHash.Should().Be(userFake.PasswordHash);
            response.PhoneNumber.Should().Be(userFake.PhoneNumber);
        }

        [Fact]
        public async Task ExecuteAsync_GivenEmptyGuid_ShouldThrowArgumentNullException()
        {
            //Arrange
            var userRepositoryFake = A.Fake<IUserRepository>();

            A.CallTo(() => userRepositoryFake.Get(Guid.Empty))
               .ThrowsAsync(() => new ArgumentNullException());

            var getUserUseCase = new GetUserUseCase(userRepositoryFake);

            //Act
            Func<Task> action = () => getUserUseCase.ExecuteAsync(Guid.Empty);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
