namespace Infrastructure.Metrics
{
    using Application.Interfaces.Infrastructure.Metrics;
    using Prometheus;

    public class PrometheusMetricsRegistry : IMetricsRegistry
    {
        private static readonly Counter GrpcCallsCount = Metrics.CreateCounter("grpc_calls", "Number of gRPC calls.");
        private static readonly Counter FailedGrpcCallsCount = Metrics.CreateCounter("failed_grpc_calls", "Number of failed gRPC calls.");
        private static readonly Histogram GrpcCallsDuration = Metrics.CreateHistogram("grpc_call_duration_milliseconds", "Histogram of gRPC calls processing durations.");

        public void CountGrpcCalls() => GrpcCallsCount.Inc();

        public void CountFailedGrpcCalls() => FailedGrpcCallsCount.Inc();

        public IMetricTimer HistogramGrpcCallsDuration() => new PrometheusTimer(GrpcCallsDuration);
    }
}
