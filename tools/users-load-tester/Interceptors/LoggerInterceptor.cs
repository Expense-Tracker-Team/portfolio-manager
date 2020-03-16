namespace UsersClient.Worker.Interceptors
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;

    public class LoggerInterceptor : Interceptor
    {
        private readonly ILogger<LoggerInterceptor> logger;

        public LoggerInterceptor(ILogger<LoggerInterceptor> logger) => this.logger = logger;

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var stopwatch = Stopwatch.StartNew();
            TResponse response = await base.UnaryServerHandler(request, context, continuation);
            stopwatch.Stop();

            // TODO: RequestHeaders
            this.logger.LogWarning($"" +
                $"GRPC call. " +
                $"Type: {context.Method}. " +
                $"Request: {typeof(TRequest)}. " +
                $"Response: {typeof(TResponse)}. " +
                $"Status code: {context.Status.StatusCode} " +
                $"Total Milliseconds: {stopwatch.Elapsed.TotalMilliseconds}");

            return response;
        }
    }
}
