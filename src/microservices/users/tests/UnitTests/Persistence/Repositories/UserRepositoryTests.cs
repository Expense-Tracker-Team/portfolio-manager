namespace UnitTests.Persistence.Repositories
{
    using FakeItEasy;
    using FluentAssertions;
    using global::Persistence.Interfaces;
    using global::Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnitTests.Common;
    using Xunit;
    using UserDataModel = global::Persistence.Models.User;

    public class UserRepositoryTests
    {
        private readonly IQueryable<UserDataModel> fakeData = new List<UserDataModel>
        {
            new UserDataModelBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3")).WithEmail("test1@test.com").Build(),
            new UserDataModelBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d4")).WithEmail("test2@test.com").Build(),
            new UserDataModelBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d5")).WithEmail("test3@test.com").Build(),
            new UserDataModelBuilder().WithId(new Guid("4beabede-1b80-4663-9a21-97e41c2616d6")).WithEmail("test4@test.com").Build(),
        }.AsQueryable();

        [Fact]
        public async Task Get_GivenUserId_ShouldReturnUser()
        {
            //Arrange
            var fakeDbSet = A.Fake<DbSet<UserDataModel>>(o => 
                o.Implements(typeof(IQueryable<UserDataModel>))
                .Implements(typeof(IAsyncEnumerable<UserDataModel>)));

            var fakeDbContext = A.Fake<IUsersDbContext>();
            A.CallTo(() => fakeDbContext.Users).Returns(fakeDbSet);
            A.CallTo(() => ((IQueryable<UserDataModel>)fakeDbSet).GetEnumerator()).Returns(fakeData.GetEnumerator());
            A.CallTo(() => ((IQueryable<UserDataModel>)fakeDbSet).Provider).Returns(fakeData.Provider);
            A.CallTo(() => ((IQueryable<UserDataModel>)fakeDbSet).Expression).Returns(fakeData.Expression);
            A.CallTo(() => ((IQueryable<UserDataModel>)fakeDbSet).ElementType).Returns(fakeData.ElementType);

            var userRepository = new UserRepository(fakeDbContext);

            //Act
            var user = await userRepository.Get(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3"));

            //Assert
            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Id.Should().Be(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3"));
            user.Email.Should().Be("test1@test.com");
        }
    }
}
