using System.Diagnostics.Metrics;
using ObservabilitySample.Service.Application.Abstractions.Metrics;

namespace ObservabilitySample.Service.Metrics;

public sealed class DiagnosticsServiceMetrics : IServiceMetrics
{
    public static readonly Meter Meter = new("ObservabilitySample.Service");

    private readonly Counter<long> _userCreatedCounter = Meter
        .CreateCounter<long>("observability_sample_service_user_created_total");

    private readonly Counter<long> _postCreatedCounter = Meter
        .CreateCounter<long>("observability_sample_service_post_created_total");

    private readonly Counter<long> _postsViewedCounter = Meter
        .CreateCounter<long>("observability_sample_service_posts_viewed_total");

    private readonly Gauge<long> _connectedUsersGauge = Meter
        .CreateGauge<long>("observability_sample_connected_user_total");

    public static readonly Histogram<double> RequestDurationHistogram = Meter
        .CreateHistogram<double>("observability_sample_request_duration_ms", unit: "ms");

    public void RecordRequestDuration(string endpointRoute, double duration)
    {
        RequestDurationHistogram.Record(
            duration,
            tags: [new KeyValuePair<string, object?>("endpoint", endpointRoute)]);
    }

    public void SetConnectedUsers(long count)
        => _connectedUsersGauge.Record(count);

    public void IncPostsViewed(int count)
        => _postsViewedCounter.Add(count);

    public void IncUserCreated()
    {
        _userCreatedCounter.Add(1);
    }

    public void IncPostCreated()
        => _postCreatedCounter.Add(1);
}
