namespace FunctionalTests.Api.Base
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using FunctionalTests.Api.Helpers;
    using Grpc.Net.Client;
    using global::Api;

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

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public void Dispose() => this.Fixture.Dispose();
    }
}