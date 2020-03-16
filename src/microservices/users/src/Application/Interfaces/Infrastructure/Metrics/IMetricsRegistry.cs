namespace Application.Interfaces.Infrastructure.Metrics
{
    public interface IMetricsRegistry
    {
        void CountGrpcCalls(string method);

        void CountFailedGrpcCalls(string method);

        void CountSuccessGrpcCalls(string method);

        IMetricTimer HistogramGrpcCallsDuration();
    }
}