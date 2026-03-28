using System.Diagnostics;
using ObservabilitySample.ServiceDefaults.Extensions;
using OpenTelemetry;

namespace ObservabilitySample.ServiceDefaults.Processors;

public sealed class OpenTelemetryTraceSuppressor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity data)
    {
        if (data.TryGetTag("rpc.service", out string? service)
            && service.Contains("opentelemetry", StringComparison.OrdinalIgnoreCase))
        {
            data.Suppress();
        }
    }
}
