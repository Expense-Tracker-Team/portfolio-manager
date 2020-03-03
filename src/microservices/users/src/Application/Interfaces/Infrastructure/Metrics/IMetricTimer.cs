namespace Application.Interfaces.Infrastructure.Metrics
{
    using System;

    public interface IMetricTimer : IDisposable
    {
        /// <summary>
        /// Observes the duration (in seconds) and returns the observed value.
        /// </summary>
        TimeSpan ObserveDuration();
    }
}
