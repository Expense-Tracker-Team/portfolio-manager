namespace Api.Interceptors
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Infrastructure.Metrics;
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;

    public class ServerInterceptor : Interceptor
    {
        private readonly ILogger<ServerInterceptor> logger;
        private readonly IMetricsRegistry metricsRegistry;

        public ServerInterceptor(ILogger<ServerInterceptor> logger, IMetricsRegistry metricsRegistry)
        {
            this.logger = logger;
            this.metricsRegistry = metricsRegistry;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            using (this.metricsRegistry.HistogramGrpcCallsDuration())
            {
                try
                {
                    TResponse response = await base.UnaryServerHandler(request, context, continuation);
                    return response;
                }
                catch (Exception ex)
                {
                    this.metricsRegistry.CountFailedGrpcCalls();

                    // TODO: Add RequestHeaders
                    this.logger.LogError($"" +
                        $"GRPC call exception. " +
                        $"Type: {context.Method}. " +
                        $"Request: {typeof(TRequest)}. " +
                        $"Response: {typeof(TResponse)}. " +
                        $"Status code: {context.Status.StatusCode} " +
                        $"Exception message: {ex.Message}");

                    if (ex is RpcException)
                    {
                        throw;
                    }

                    throw new RpcException(new Status(StatusCode.Internal, string.Empty));
                }
                finally
                {
                    this.metricsRegistry.CountGrpcCalls();
                }
            }
        }
    }
}