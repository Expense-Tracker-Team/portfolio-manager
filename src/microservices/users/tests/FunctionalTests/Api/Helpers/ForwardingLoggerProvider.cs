namespace FunctionalTests.Api.Helpers
{
    using System;
    using Microsoft.Extensions.Logging;

    internal class ForwardingLoggerProvider : ILoggerProvider
    {
        private readonly LogMessage logAction;

        public ForwardingLoggerProvider(LogMessage logAction) => this.logAction = logAction;

        public ILogger CreateLogger(string categoryName) => new ForwardingLogger(categoryName, this.logAction);

        public void Dispose()
        {
        }

        internal class ForwardingLogger : ILogger
        {
            private readonly string categoryName;
            private readonly LogMessage logAction;

            public ForwardingLogger(string categoryName, LogMessage logAction)
            {
                this.categoryName = categoryName;
                this.logAction = logAction;
            }

            public IDisposable? BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => this.logAction(logLevel, this.categoryName, eventId, formatter(state, exception), exception);
        }
    }
}