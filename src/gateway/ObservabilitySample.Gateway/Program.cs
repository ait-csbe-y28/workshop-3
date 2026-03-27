using ObservabilitySample.Gateway.Application;
using ObservabilitySample.Gateway.Infrastructure.Service;
using ObservabilitySample.Gateway.Presentation.Http;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Enrichers.ActivityTags;
using Serilog.Enrichers.Span;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

Log.Logger = new LoggerConfiguration()
    .WriteTo.OpenTelemetry()
    .WriteTo.Console()
    .Enrich.WithActivityTags()
    .Enrich.WithSpan()
    .CreateLogger();

builder.Services.AddLogging(logging => logging.ClearProviders().AddSerilog());

builder.Services
    .AddApplication()
    .AddInfrastructureService()
    .AddPresentationHttp();

builder.Services
    .AddOpenApi()
    .AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Infrastructure:Caching:Redis:ConnectionString"];
});

builder.Services.AddHybridCache();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseRouting();
app.UsePresentationHttp();

app.Run();
