namespace FunctionalTests.Api.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.Extensions.Logging;

    internal class GrpcTestContext<TStartup> : IDisposable where TStartup : class
    {
        private readonly ExecutionContext executionContext;
        private readonly Stopwatch stopwatch;
        private readonly GrpcTestFixture<TStartup> fixture;

        public GrpcTestContext(GrpcTestFixture<TStartup> fixture)
        {
            this.executionContext = ExecutionContext.Capture()!;
            this.stopwatch = Stopwatch.StartNew();
            this.fixture = fixture;
            this.fixture.LoggedMessage += this.WriteMessage;
        }

        private void WriteMessage(LogLevel logLevel, string category, EventId eventId, string message, Exception exception)
        {
            // Log using the passed in execution context.
            // In the case of NUnit, console output is only captured by the test
            // if it is written in the test's execution context.
            ExecutionContext.Run(this.executionContext, s => Console.WriteLine($"{this.stopwatch.Elapsed.TotalSeconds:N3}s {category} - {logLevel}: {message}"), null);
        }

        public void Dispose()
        {
            this.fixture.LoggedMessage -= this.WriteMessage;
            this.executionContext?.Dispose();
        }
    }
}