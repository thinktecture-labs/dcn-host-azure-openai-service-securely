using HostingAzureOpenAiSample.WebApp.OpenAiAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HostingAzureOpenAiSample.WebApp.CompositionRoot;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.Host.UseSerilog(logger);
        builder
           .Services
           .AddOpenAiAccess(builder.Configuration)
           .AddSpaIfNecessary(builder.Environment)
           .AddCorsIfNecessary(builder.Environment)
           .AddHealthChecks();
        return builder;
    }
}