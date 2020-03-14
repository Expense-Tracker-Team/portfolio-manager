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
                this.metricsRegistry.CountGrpcCalls(context.Method);

            this.logger.LogInformation("Unary GRPC call. " +
                "Method: {Method}. " +
                "Request: {RequestType}. " +
                "Request data: {@RequestData} " +
                "Response: {ResponseType}. ", context.Method, typeof(TRequest), request, typeof(TResponse));

            using (this.metricsRegistry.HistogramGrpcCallsDuration())
            {
                try
                {
                    TResponse response = await base.UnaryServerHandler(request, context, continuation);
                    this.metricsRegistry.CountSuccessGrpcCalls(context.Method);
                    return response;
                }
                catch (Exception ex)
                {
                    this.metricsRegistry.CountFailedGrpcCalls(context.Method);

                    // TODO: Add RequestHeaders
                    this.logger.LogError("Unary GRPC call exception. " +
                        "Method: {Method}. " +
                        "Request: {RequestType}. " +
                        "Request data: {@RequestData}" +
                        "Response: {ResponseType}. " +
                        "Status code: {StatusCode} " +
                        "Exception message: {@Exception}", context.Method, typeof(TRequest), request, typeof(TResponse), context.Status.StatusCode, ex);

                    if (ex is RpcException)
                    {
                        throw;
                    }

                    throw new RpcException(new Status(StatusCode.Internal, string.Empty));
                }
            }
        }
    }
}