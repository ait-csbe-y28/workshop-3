using Itmo.Dev.Platform.Common.Extensions;
using Npgsql;
using ObservabilitySample.Service.Application;
using ObservabilitySample.Service.Application.Abstractions.Metrics;
using ObservabilitySample.Service.HealthChecks;
using ObservabilitySample.Service.Infrastructure.Persistence;
using ObservabilitySample.Service.Metrics;
using ObservabilitySample.Service.Presentation.Grpc;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.DependencyInjection;
using Serilog;
using Serilog.Enrichers.ActivityTags;
using Serilog.Enrichers.Span;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlatform(platform => platform
    .WithNewtonsoftSerialization());

builder.Services
    .AddApplication()
    .AddInfrastructurePersistence()
    .AddPresentationGrpc();

if (builder.Configuration.GetValue<bool>("USE_PROMETHEUS_METRICS"))
{
    builder.Services.AddMetricFactory();
    builder.Services.AddSingleton<IServiceMetrics, PrometheusServiceMetrics>();
}
else
{
    builder.Services.AddSingleton<IServiceMetrics, DiagnosticsServiceMetrics>();
}

builder.Services
    .AddOpenTelemetry()
    .WithMetrics(metrics => metrics
        .AddMeter(DiagnosticsServiceMetrics.Meter.Name)
        .AddNpgsqlInstrumentation())
    .WithTracing(tracing => tracing
        .AddNpgsql()
        .AddProcessor(new PostgresTraceSuppressor()));

builder.AddServiceDefaults();

Log.Logger = new LoggerConfiguration()
    .WriteTo.OpenTelemetry()
    .WriteTo.Console()
    .Enrich.WithActivityTags()
    .Enrich.WithSpan()
    .CreateLogger();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics => metrics
        .AddMeter(DiagnosticsServiceMetrics.Meter.Name)
        .AddView(
            DiagnosticsServiceMetrics.RequestDurationHistogram.Name,
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = [10, 100, 500, 1000],
            }));

builder.Services.AddLogging(logging => logging.ClearProviders().AddSerilog());

builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: ["health"]);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (builder.Configuration.GetValue<bool>("USE_PROMETHEUS_METRICS"))
{
    app.UsePrometheusServer();
}

app.UseRouting();
app.UsePresentationGrpc();

app.Run();
