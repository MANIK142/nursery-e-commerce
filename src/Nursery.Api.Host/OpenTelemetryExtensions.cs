using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Nursery.Api.Host;

public static class OpenTelemetryExtensions
{
    private const string ServiceName = "Nursery.Api.Host";

    public static IServiceCollection AddNurseryObservability(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                                                .AddService(serviceName: ServiceName, serviceVersion: "1.0.0"))
                                                .WithTracing(tracing => tracing
                                                .AddAspNetCoreInstrumentation(options =>
                                                {
                                                    options.Filter = httpContext =>
                                                        !httpContext.Request.Path.StartsWithSegments("/health");
                                                })
                                                .AddHttpClientInstrumentation()
                                                .AddEntityFrameworkCoreInstrumentation(options =>
                                                {
                                                    options.EnrichWithIDbCommand = (activity, command) =>
                                                    {
                                                        activity.SetTag("db.statement", command.CommandText);
                                                    };
                                                })
                                                .AddSource("Nursery.*")
                                                .AddOtlpExporter(otlp =>
                                                {
                                                    otlp.Endpoint = new Uri(configuration["OpenTelemetry:OtlpEndpoint"]!);
                                                }
                                                )
                                             ).WithMetrics(metrics => metrics
                                                      .AddAspNetCoreInstrumentation()      // request duration, count, active requests
                                                      .AddHttpClientInstrumentation()      // outbound call metrics — pairs with your new Polly work
                                                      .AddRuntimeInstrumentation()         // GC, thread pool, exceptions/sec
                                                      .AddPrometheusExporter()); 

         return services;
    }
}
