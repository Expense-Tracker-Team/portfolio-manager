namespace IntegrationTests.Persistence.Repositories
{
    using Domain;
    using DotNet.Testcontainers.Containers.Builders;
    using DotNet.Testcontainers.Containers.Configurations.Databases;
    using DotNet.Testcontainers.Containers.Modules.Databases;
    using FluentAssertions;
    using global::Persistence;
    using global::Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using System.Collections.Generic;
    using UserDataModel = global::Persistence.Models.User;
    using Xunit.Abstractions;

    public class UserRepositoryTests
    {
        private readonly ITestOutputHelper output;

        public UserRepositoryTests(ITestOutputHelper output) => this.output = output;

        [Fact]
        public async Task CreateAsync_WithValidUser_ShouldAddToDatabase()
        {
            // Arrange
            const string databaseName = "users";
            const string username = "postgres";
            const string password = "postgres";

            ITestcontainersBuilder<PostgreSqlTestcontainer> testContainersBuilder = new TestcontainersBuilder<PostgreSqlTestcontainer>()
              .WithDatabase(new PostgreSqlTestcontainerConfiguration
              {
                  Database = databaseName,
                  Username = username,
                  Password = password,
              });

            await using PostgreSqlTestcontainer postgresContainer = testContainersBuilder.Build();
            await postgresContainer.StartAsync();

            this.output.WriteLine($"Started Postgres test container");

            DbContextOptions<UsersDbContext> postgresOptions = new DbContextOptionsBuilder<UsersDbContext>()
                .UseNpgsql(postgresContainer.ConnectionString)
                .Options;

            using var dbContext = new UsersDbContext(postgresOptions);

            dbContext.Database.EnsureCreated();
            this.output.WriteLine($"Database created");

            var userRepository = new UserRepository(dbContext);

            var userId = Guid.NewGuid();

            User user = new UserBuilder()
                .WithId(userId)
                .Build();

            // Act 
            await userRepository.CreateAsync(user);

            // Assert
            List<UserDataModel> allUsers = await dbContext.Users.ToListAsync();
            allUsers.Count.Should().Be(1);
            allUsers[0].Id.Should().Be(userId);
        }

        [Fact]
        public async Task GetAsync_GivenValidId_ShouldReturnUserFromTheDatabase()
        {
            // Arrange
            const string databaseName = "users";
            const string username = "postgres";
            const string password = "postgres";

            ITestcontainersBuilder<PostgreSqlTestcontainer> testContainersBuilder = new TestcontainersBuilder<PostgreSqlTestcontainer>()
              .WithDatabase(new PostgreSqlTestcontainerConfiguration
              {
                  Database = databaseName,
                  Username = username,
                  Password = password,
              });

            await using PostgreSqlTestcontainer postgresContainer = testContainersBuilder.Build();
            await postgresContainer.StartAsync();

            this.output.WriteLine($"Started Postgres test container");

            DbContextOptions<UsersDbContext> postgresOptions = new DbContextOptionsBuilder<UsersDbContext>()
                .UseNpgsql(postgresContainer.ConnectionString)
                .Options;

            using var dbContext = new UsersDbContext(postgresOptions);

            dbContext.Database.EnsureCreated();
            this.output.WriteLine($"Database created");

            var userRepository = new UserRepository(dbContext);

            var userId = Guid.NewGuid();

            User user = new UserBuilder()
                .WithId(userId)
                .Build();

            var dbUser = await userRepository.CreateAsync(user);

            // Act 
            var response = await userRepository.GetAsync(dbUser.Id);

            // Assert
            response.Id.Should().NotBeEmpty();
            response.Id.Should().Be(dbUser.Id);
            response.Name.Should().Be(dbUser.Name);
            response.PhoneNumber.Should().Be(dbUser.PhoneNumber);
            response.PasswordHash.Should().Be(dbUser.PasswordHash);
        }
    }
}
