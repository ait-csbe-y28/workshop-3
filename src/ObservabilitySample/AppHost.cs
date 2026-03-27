IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("service-postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("service-postgres-volume");

IResourceBuilder<PostgresDatabaseResource> database = postgres.AddDatabase("postgres");

IResourceBuilder<RedisResource> redis = builder.AddRedis("redis");

IResourceBuilder<ProjectResource> service = builder.AddProject<Projects.ObservabilitySample_Service>("service")
    .WithEnvironment(
        "Infrastructure:Persistence:Postgres:Host",
        postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Host))
    .WithEnvironment(
        "Infrastructure:Persistence:Postgres:Port",
        postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Port))
    .WithEnvironment(
        "Infrastructure:Persistence:Postgres:Database",
        database.Resource.DatabaseName)
    .WithEnvironment(
        "Infrastructure:Persistence:Postgres:Username",
        postgres.Resource.UserNameReference)
    .WithEnvironment(
        "Infrastructure:Persistence:Postgres:Password",
        postgres.Resource.PasswordParameter)
    .WithEnvironment("USE_PROMETHEUS_METRICS", "true")
    .WithHttpHealthCheck("/health");

IResourceBuilder<ProjectResource> gateway = builder.AddProject<Projects.ObservabilitySample_Gateway>("gateway")
    .WaitFor(service)
    .WithEnvironment(
        "Infrastructure:Service:service-user:Address",
        service.GetEndpoint("gRPC"))
    .WithEnvironment(
        "Infrastructure:Service:service-posts:Address",
        service.GetEndpoint("gRPC"))
    .WithEnvironment(
        "Infrastructure:Caching:Redis:ConnectionString",
        redis.Resource.ConnectionStringExpression);

builder.Build().Run();
