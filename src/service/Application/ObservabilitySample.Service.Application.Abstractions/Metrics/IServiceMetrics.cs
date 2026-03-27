namespace ObservabilitySample.Service.Application.Abstractions.Metrics;

public interface IServiceMetrics
{
    void IncUserCreated();

    void IncPostCreated();

    void IncPostsViewed(int count);
}
