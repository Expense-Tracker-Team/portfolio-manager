namespace Application.Infrastructure.Metrics
{
    public interface IMetricsRegistry
    {
        void CountGrpcCalls(string method, string statusCode);

        void CountFailedGrpcCalls(string method);

        void CountSuccessGrpcCalls(string method);

        IMetricTimer HistogramGrpcCallsDuration();
    }
}