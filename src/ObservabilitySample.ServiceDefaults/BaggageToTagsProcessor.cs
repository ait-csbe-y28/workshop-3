using System.Diagnostics;
using OpenTelemetry;

namespace Microsoft.Extensions.Hosting;

public sealed class BaggageToTagsProcessor : BaseProcessor<Activity>
{
    public override void OnStart(Activity data)
    {
        foreach (KeyValuePair<string, string?> pair in data.Baggage)
        {
            data.SetTag(pair.Key, pair.Value);
        }
    }
}
