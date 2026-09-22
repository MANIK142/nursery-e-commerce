using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Nursery.Catalog.Api.Endpoints.Plant;

internal static class PlantApiVersioning
{
    private static ApiVersionSet? _versionSet;

    public static ApiVersionSet VersionSet(IEndpointRouteBuilder app) =>
        _versionSet ??= app.NewApiVersionSet("Catalog.Plants")
                        .HasApiVersion(new ApiVersion(1, 0))
                        .HasApiVersion(new ApiVersion(2, 0))
                        .ReportApiVersions()
                        .Build();
}