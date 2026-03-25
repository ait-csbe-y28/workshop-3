using ObservabilitySample.Gateway.Application;
using ObservabilitySample.Gateway.Infrastructure.Service;
using ObservabilitySample.Gateway.Presentation.Http;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructureService()
    .AddPresentationHttp();

builder.Services
    .AddOpenApi()
    .AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseRouting();
app.UsePresentationHttp();

app.Run();
