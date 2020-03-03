namespace FunctionalTests.Api.Helpers
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using global::Api;

    public delegate void LogMessage(LogLevel logLevel, string categoryName, EventId eventId, string message, Exception exception);

    public class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer server;
        private readonly IHost host;

        public event LogMessage? LoggedMessage;

        public GrpcTestFixture() : this(null) { }

        public GrpcTestFixture(Action<IServiceCollection>? initialConfigureServices)
        {
            this.LoggerFactory = new LoggerFactory();
            this.LoggerFactory.AddProvider(new ForwardingLoggerProvider((logLevel, category, eventId, message, exception) => LoggedMessage?.Invoke(logLevel, category, eventId, message, exception)));

            IHostBuilder builder = new HostBuilder()
                .ConfigureServices(services =>
                {
                    initialConfigureServices?.Invoke(services);
                    services.AddSingleton<ILoggerFactory>(this.LoggerFactory);
                })
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost
                        .UseTestServer()
                        .UseStartup<TStartup>();
                });

            this.host = builder.Start();
            this.server = this.host.GetTestServer();

            // Need to set the response version to 2.0.
            // Required because of this TestServer issue - https://github.com/aspnet/AspNetCore/issues/16940
            var responseVersionHandler = new ResponseVersionHandler
            {
                InnerHandler = this.server.CreateHandler()
            };

            var client = new HttpClient(responseVersionHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            this.Client = client;
        }

        public LoggerFactory LoggerFactory { get; }

        public HttpClient Client { get; }

        public void Dispose()
        {
            this.Client.Dispose();
            this.host.Dispose();
            this.server.Dispose();
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;

                return response;
            }
        }

        public IDisposable GetTestContext() => new GrpcTestContext<TStartup>(this);

        public static implicit operator GrpcTestFixture<TStartup>(GrpcTestFixture<Startup> testFixture) => throw new NotImplementedException();
    }
}