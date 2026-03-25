using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ObservabilitySample.Gateway.Application.Abstractions.Posts;
using ObservabilitySample.Gateway.Application.Abstractions.Users;
using ObservabilitySample.Gateway.Infrastructure.Service.Options;
using ObservabilitySample.Gateway.Infrastructure.Service.Posts;
using ObservabilitySample.Gateway.Infrastructure.Service.Users;
using ObservabilitySample.Service.Proto;

namespace ObservabilitySample.Gateway.Infrastructure.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection collection)
    {
        const string postsServiceName = "service-posts";
        const string userServiceName = "service-user";

        collection
            .AddOptions<GrpcClientOptions>(postsServiceName)
            .BindConfiguration($"Infrastructure:Service:{postsServiceName}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        collection
            .AddOptions<GrpcClientOptions>(userServiceName)
            .BindConfiguration($"Infrastructure:Service:{userServiceName}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        collection
            .AddGrpcClient<PostsService.PostsServiceClient>(postsServiceName)
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();
                client.BaseAddress = options.Get(postsServiceName).Address;
            });

        collection
            .AddGrpcClient<UserService.UserServiceClient>(userServiceName)
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<GrpcClientOptions>>();
                client.BaseAddress = options.Get(userServiceName).Address;
            });

        collection.AddScoped<IPostClient, PostClient>();
        collection.AddScoped<IUserClient, UserClient>();

        return collection;
    }
}
