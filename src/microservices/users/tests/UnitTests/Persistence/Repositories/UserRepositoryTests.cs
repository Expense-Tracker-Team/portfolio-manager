namespace UnitTests.Persistence.Repositories
{
    using FakeItEasy;
    using FluentAssertions;
    using global::Persistence;
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

            var dbOptions = new DbContextOptionsBuilder<UsersDbContext>()
                  .UseInMemoryDatabase(databaseName: "UsersDbContext")
                  .Options;
  
            using (var context = new UsersDbContext(dbOptions))
            {
                context.Users.AddRange(fakeData);
                context.SaveChanges();
            }

            using (var context = new UsersDbContext(dbOptions))
            {
                var repository = new UserRepository(context);

                var response = await repository.Get(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3"));

                response.Id.Should().NotBeEmpty();
                response.Id.Should().Be(new Guid("4beabede-1b80-4663-9a21-97e41c2616d3"));
                response.Email.Should().Be("test1@test.com");
            }
        }
    }
}
