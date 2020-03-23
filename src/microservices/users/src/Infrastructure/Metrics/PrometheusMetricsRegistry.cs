namespace Infrastructure.Metrics
{
    using Application.Interfaces.Infrastructure.Metrics;
    using Prometheus;

    // In case you are hesitating how to name your metric, consult https://prometheus.io/docs/practices/naming/
    public class PrometheusMetricsRegistry : IMetricsRegistry
    {
        private static readonly Counter GrpcCallsCount = Metrics.CreateCounter("grpc_calls_total", "Number of gRPC calls.", new CounterConfiguration
        {
            LabelNames = new[] {
                "method",
                "status_code"
            }
        });

        private static readonly Counter GrpcSuccessCallsCount = Metrics.CreateCounter("grpc_success_calls_total", "Number of success gRPC calls.", new CounterConfiguration
        {
            LabelNames = new[] {
                "method"
            }
        });

        private static readonly Counter GrpcFailedCallsCount = Metrics.CreateCounter("grpc_failed_calls_total", "Number of failed gRPC calls.", new CounterConfiguration
        {
            LabelNames = new[] {
                "method"
            }
        });

        private static readonly Histogram GrpcCallsDuration = Metrics.CreateHistogram("grpc_call_duration_milliseconds", "Histogram of gRPC calls processing durations.");

        public void CountGrpcCalls(string method, string statusCode) => GrpcCallsCount.WithLabels(method, statusCode).Inc();

        public void CountSuccessGrpcCalls(string method) => GrpcSuccessCallsCount.WithLabels(method).Inc();

        public void CountFailedGrpcCalls(string method) => GrpcFailedCallsCount.WithLabels(method).Inc();

        public IMetricTimer HistogramGrpcCallsDuration() => new PrometheusTimer(GrpcCallsDuration);
    }
}
