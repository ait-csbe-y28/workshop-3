using ObservabilitySample.Service.Presentation.Grpc.Controllers;

namespace ObservabilitySample.Service.Presentation.Grpc;

public static class WebApplicationExtensions
{
    public static WebApplication UsePresentationGrpc(this WebApplication application)
    {
        application.MapGrpcService<UserController>();
        application.MapGrpcService<PostController>();
        application.MapGrpcReflectionService();

        return application;
    }
}
