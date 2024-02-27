using Microsoft.AspNetCore.Builder;

namespace HostingAzureOpenAiSample.WebApp.CompositionRoot;

public static class HealthChecks
{
    public static WebApplication MapReadinessAndLivenessHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health/readiness");
        app.MapHealthChecks("/health/liveness");
        return app;
    }
}