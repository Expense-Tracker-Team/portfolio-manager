namespace Application.Interfaces.Infrastructure.Metrics
{
    public interface IMetricsRegistry
    {
        void CountGrpcCalls();

        void CountFailedGrpcCalls();

        IMetricTimer HistogramGrpcCallsDuration();
    }
}