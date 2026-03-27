using ObservabilitySample.Service.Application.Abstractions.Metrics;
using Prometheus.Client;

namespace ObservabilitySample.Service.Metrics;

public sealed class PrometheusServiceMetrics(IMetricFactory metricFactory) : IServiceMetrics
{
    private readonly ICounter<long> _userCreatedCounter = metricFactory.CreateCounterInt64(
        name: "observability_sample_service_user_created_total",
        help: "Total number of created users");

    private readonly ICounter<long> _postCreatedCounter = metricFactory.CreateCounterInt64(
        name: "observability_sample_service_post_created_total",
        help: "Total count of created posts");

    private readonly ICounter<long> _postsViewedCounter = metricFactory.CreateCounterInt64(
        name: "observability_sample_service_posts_viewed_total",
        help: "Total count of post views");

    private readonly IMetricFamily<IHistogram> _requestDurationHistogram = metricFactory.CreateHistogram(
        name: "observability_sample_request_duration_ms",
        help: "Duration of requests",
        labelNames: ["endpoint"],
        buckets: [10, 100, 500, 1000]);

    public void RecordRequestDuration(string endpointRoute, double duration)
    {
        _requestDurationHistogram
            .WithLabels(endpointRoute)
            .Observe(duration);
    }

    public void IncPostsViewed(int count)
        => _postsViewedCounter.Inc(count);

    public void IncUserCreated()
        => _userCreatedCounter.Inc();

    public void IncPostCreated()
        => _postCreatedCounter.Inc();
}
