using Itmo.Dev.Platform.Persistence.Abstractions.Extensions;
using Itmo.Dev.Platform.Persistence.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ObservabilitySample.Service.Application.Abstractions.Persistence;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;
using ObservabilitySample.Service.Infrastructure.Persistence.Repositories;

namespace ObservabilitySample.Service.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddPlatformPersistence(persistence => persistence.UsePostgres(postgres => postgres
            .WithConnectionOptions(builder => builder
                .BindConfiguration("Infrastructure:Persistence:Postgres"))
            .WithMigrationsFrom(typeof(IAssemblyMarker).Assembly)));

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IPostRepository, PostRepository>();

        return collection;
    }
}
