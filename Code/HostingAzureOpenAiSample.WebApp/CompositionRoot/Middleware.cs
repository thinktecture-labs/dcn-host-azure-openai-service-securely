using HostingAzureOpenAiSample.WebApp.Chat;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace HostingAzureOpenAiSample.WebApp.CompositionRoot;

public static class Middleware
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseCorsIfNecessary();
        app.UseSpaStaticFilesIfNecessary(app.Environment);
        app.UseRouting();
        app.MapReadinessAndLivenessHealthChecks();
        app.MapOpenAiChat();
        app.UseSpaIfNecessary(app.Environment);
        
        return app;
    }
}