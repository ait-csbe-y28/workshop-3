namespace ObservabilitySample.Gateway.Presentation.Http;

public static class WebApplicationExtensions
{
    public static WebApplication UsePresentationHttp(this WebApplication application)
    {
        application.MapControllers();
        return application;
    }
}
