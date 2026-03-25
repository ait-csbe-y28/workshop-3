using Microsoft.Extensions.DependencyInjection;
using ObservabilitySample.Gateway.Application.Contracts.Posts;
using ObservabilitySample.Gateway.Application.Contracts.Users;
using ObservabilitySample.Gateway.Application.Posts;
using ObservabilitySample.Gateway.Application.Users;

namespace ObservabilitySample.Gateway.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IUserService, UserService>();
        collection.AddScoped<IPostService, PostService>();

        return collection;
    }
}
