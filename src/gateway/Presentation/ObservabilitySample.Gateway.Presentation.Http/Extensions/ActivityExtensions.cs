using System.Diagnostics;

namespace ObservabilitySample.Gateway.Presentation.Http.Extensions;

public static class ActivityExtensions
{
    public static void AddUserIdBaggage(this Activity? activity, long userId)
    {
        activity?.AddTag("user.id", userId.ToString());
        activity?.AddBaggage("user.id", userId.ToString());
    }
}
