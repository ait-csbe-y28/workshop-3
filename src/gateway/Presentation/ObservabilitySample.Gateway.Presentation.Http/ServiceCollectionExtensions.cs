namespace ObservabilitySample.Gateway.Presentation.Http;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationHttp(this IServiceCollection collection)
    {
        collection.AddControllers();
        return collection;
    }
}
