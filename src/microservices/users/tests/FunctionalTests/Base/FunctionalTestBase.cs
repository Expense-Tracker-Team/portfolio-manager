namespace FunctionalTests.Api.Base
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using FunctionalTests.Api.Helpers;
    using Grpc.Net.Client;
    using global::Api;
    using DotNet.Testcontainers.Containers.Modules.Databases;
    using DotNet.Testcontainers.Containers.Builders;
    using DotNet.Testcontainers.Containers.Configurations.Databases;
    using Persistence;
    using Microsoft.EntityFrameworkCore;

    // https://github.com/grpc/grpc-dotnet/blob/master/examples/Tester/Tests/FunctionalTests/FunctionalTestBase.cs
    public class FunctionalTestBase : IDisposable
    {
        private GrpcChannel? channel;

        public FunctionalTestBase() => this.Fixture = new GrpcTestFixture<Startup>(this.ConfigureServices);

        protected GrpcTestFixture<Startup> Fixture { get; private set; } = default!;

        protected ILoggerFactory LoggerFactory => this.Fixture.LoggerFactory;

        protected GrpcChannel Channel => this.channel ??= this.CreateChannel();

        protected GrpcChannel CreateChannel()
        {
            return GrpcChannel.ForAddress(this.Fixture.Client.BaseAddress, new GrpcChannelOptions
            {
                LoggerFactory = LoggerFactory,
                HttpClient = this.Fixture.Client
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services) => InitializeDatabase(services);

        private static void InitializeDatabase(IServiceCollection services)
        {
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

            PostgreSqlTestcontainer postgresContainer = testContainersBuilder.Build();
            postgresContainer.StartAsync().Wait();

            services.AddDbContext<UsersDbContext>(options =>
                    options.UseNpgsql(postgresContainer.ConnectionString));

            services.BuildServiceProvider().GetRequiredService<UsersDbContext>().Database.EnsureCreated();
        }

        public void Dispose() => this.Fixture.Dispose();
    }
}