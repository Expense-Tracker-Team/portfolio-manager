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
            var user = new UserBuilder().Build();
            var userRepositoryStub = A.Fake<IUserRepository>();

            A.CallTo(() => userRepositoryStub.GetAsync(A<Guid>.Ignored))
                .Returns(user);

            var getUserUseCase = new GetUserUseCase(userRepositoryStub);

            //Act
            var response = await getUserUseCase.ExecuteAsync(Guid.NewGuid());

            //Assert
            response.Id.Should().NotBe(Guid.Empty);
            response.Name.Should().Be(user.Name);
            response.Email.Should().Be(user.Email);
            response.PasswordHash.Should().Be(user.PasswordHash);
            response.PhoneNumber.Should().Be(user.PhoneNumber);
        }

        [Fact]
        public void ExecuteAsync_GivenEmptyGuid_ShouldThrowArgumentNullException()
        {
            //Arrange
            var userRepositoryStub = A.Fake<IUserRepository>();

            A.CallTo(() => userRepositoryStub.GetAsync(Guid.Empty))
               .ThrowsAsync(() => new ArgumentNullException());

            var getUserUseCase = new GetUserUseCase(userRepositoryStub);

            //Act
            Func<Task> action = () => getUserUseCase.ExecuteAsync(Guid.Empty);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
