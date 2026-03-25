using Itmo.Dev.Platform.Common.Extensions;
using ObservabilitySample.Service.Application;
using ObservabilitySample.Service.Infrastructure.Persistence;
using ObservabilitySample.Service.Presentation.Grpc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlatform(platform => platform
    .WithNewtonsoftSerialization());

builder.Services
    .AddApplication()
    .AddInfrastructurePersistence()
    .AddPresentationGrpc();

WebApplication app = builder.Build();

app.UseRouting();
app.UsePresentationGrpc();

app.Run();
