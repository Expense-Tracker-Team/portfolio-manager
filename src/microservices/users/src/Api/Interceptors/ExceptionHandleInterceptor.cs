namespace Api.Interceptors
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;

    public class ExceptionHandleInterceptor : Interceptor
    {
        private readonly ILogger<LoggerInterceptor> logger;

        public ExceptionHandleInterceptor(ILogger<LoggerInterceptor> logger) => this.logger = logger;

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                TResponse response = await base.UnaryServerHandler(request, context, continuation);
                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // TODO: Add RequestHeaders
                this.logger.LogError($"" +
                    $"GRPC call exception. " +
                    $"Type: {context.Method}. " +
                    $"Request: {typeof(TRequest)}. " +
                    $"Response: {typeof(TResponse)}. " +
                    $"Status code: {context.Status.StatusCode} " +
                    $"Total Milliseconds: {stopwatch.Elapsed.TotalMilliseconds} " +
                    $"Exception message: {ex.Message}");

                if (ex is RpcException)
                {
                    throw;
                }

                throw new RpcException(new Status(StatusCode.Internal, string.Empty));
            }
        }
    }
}