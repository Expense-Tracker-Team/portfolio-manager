namespace UnitTests.Application.UseCases
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
        private readonly IUserRepository userRepositoryFake;

        public GetUserUseCaseTests()
        {
            this.userRepositoryFake = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task ExecuteAsync_GivenUserId_ShouldReturnUser()
        {
            //Arrange
            var userFake = new UserBuilder().Build();
            var getUserUseCaseMock = new GetUserUseCase(this.userRepositoryFake);

            A.CallTo(() => this.userRepositoryFake.Get(A<Guid>.Ignored))
                .Returns(userFake);

            //Act
            var response = await getUserUseCaseMock.ExecuteAsync(Guid.NewGuid());

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
            var getUserUseCaseMock = new GetUserUseCase(this.userRepositoryFake);
            A.CallTo(() => this.userRepositoryFake.Get(Guid.Empty))
               .ThrowsAsync(new ArgumentNullException());

            //Act
            Func<Task> action = () => getUserUseCaseMock.ExecuteAsync(Guid.Empty);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
