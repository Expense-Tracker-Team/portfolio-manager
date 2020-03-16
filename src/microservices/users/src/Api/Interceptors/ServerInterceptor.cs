namespace Api.Interceptors
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Constants;
    using Application.Interfaces.Infrastructure.Metrics;
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class ServerInterceptor : Interceptor
    {
        private readonly ILogger<ServerInterceptor> logger;
        private readonly IMetricsRegistry metricsRegistry;

        // TODO: Consider logging sensitive data -> {request}
        // TODO: Add RequestHeaders
        private const string InformationLogTemplate = "Unary GRPC call to {Method} with {RequestType}, {@RequestData} and {ResponseType}";
        private const string ErrorLogTemplate = "Unary GRPC call exception to {Method} with {RequestType}, {@RequestData} and {ResponseType} throws {@Exception} and has {StatusCode}";

        public ServerInterceptor(ILogger<ServerInterceptor> logger, IMetricsRegistry metricsRegistry)
        {
            this.logger = logger;
            this.metricsRegistry = metricsRegistry;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var correlationId = context.RequestHeaders.FirstOrDefault(h => h.Key.Equals(GrpcMetadata.CorrelationIdRequestHeaderKey, StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            using (this.metricsRegistry.HistogramGrpcCallsDuration())
            {
                using (LogContext.PushProperty(SerilogCustomProperties.CorrelationId, correlationId))
                {
                    this.logger.LogInformation(InformationLogTemplate, context.Method, typeof(TRequest), request, typeof(TResponse));

                    var statusCode = context.Status.StatusCode;
                    try
                    {
                        TResponse response = await base.UnaryServerHandler(request, context, continuation);
                        this.metricsRegistry.CountSuccessGrpcCalls(context.Method);
                        return response;
                    }
                    catch (Exception ex)
                    {
                        this.metricsRegistry.CountFailedGrpcCalls(context.Method);
                        this.logger.LogError(ErrorLogTemplate, context.Method, typeof(TRequest), request, typeof(TResponse), ex, context.Status.StatusCode);

                        if (ex is RpcException)
                        {
                            statusCode = ((RpcException)ex).Status.StatusCode;
                            throw;
                        }

                        statusCode = StatusCode.Internal;
                        throw new RpcException(new Status(StatusCode.Internal, string.Empty));
                    }
                    finally
                    {
                        this.metricsRegistry.CountGrpcCalls(context.Method, statusCode.ToString());
                    }
                }

            }
        }
    }
}