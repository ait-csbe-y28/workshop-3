using Grpc.Net.Client;
using Itmo.Dev.Platform.Testing.ApplicationFactories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace IntegrationalTests.Fixtures;

public sealed class WebApplicationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:latest").Build();

    private WebApplicationFactory<Program> _webApplicationFactory = null!;

    public IServiceProvider Services => _webApplicationFactory.Services;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        _webApplicationFactory = new PlatformWebApplicationBuilder<Program>()
            .ConfigureConfiguration(builder => builder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Infrastructure:Persistence:Postgres:Host", _container.Hostname },
                {
                    "Infrastructure:Persistence:Postgres:Port",
                    _container.GetMappedPublicPort(5432).ToString()
                },
                { "Infrastructure:Persistence:Postgres:Database", "postgres" },
                { "Infrastructure:Persistence:Postgres:Username", "postgres" },
                { "Infrastructure:Persistence:Postgres:Password", "postgres" },
                { "Infrastructure:Persistence:Postgres:SslMode", "Prefer" },
            }))
            .Build();

        _webApplicationFactory.StartServer();
    }

    public async Task DisposeAsync()
    {
        await _webApplicationFactory.DisposeAsync();
        await _container.DisposeAsync();
    }

    public GrpcChannel CreateChannel()
    {
        var grpcChannelOptions = new GrpcChannelOptions
        {
            HttpHandler = _webApplicationFactory.Server.CreateHandler(),
        };

        return GrpcChannel.ForAddress("http://localhost", grpcChannelOptions);
    }
}
