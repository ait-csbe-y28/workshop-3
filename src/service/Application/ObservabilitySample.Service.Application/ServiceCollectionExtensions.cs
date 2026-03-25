using Microsoft.Extensions.DependencyInjection;
using ObservabilitySample.Service.Application.Contracts.Posts;
using ObservabilitySample.Service.Application.Contracts.Users;
using ObservabilitySample.Service.Application.Posts;
using ObservabilitySample.Service.Application.Users;

namespace ObservabilitySample.Service.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IUserService, UserService>();
        collection.AddScoped<IPostService, PostService>();

        return collection;
    }
}
