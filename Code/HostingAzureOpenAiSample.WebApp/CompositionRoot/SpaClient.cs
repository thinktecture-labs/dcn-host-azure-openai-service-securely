using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostingAzureOpenAiSample.WebApp.CompositionRoot;

public static class SpaClient
{
    private const string WebClientDirectory = "bundled-angular-client";
    
    public static IServiceCollection AddSpaIfNecessary(this IServiceCollection services, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
            services.AddSpaStaticFiles(options => options.RootPath = WebClientDirectory);
        return services;
    }
    
    public static WebApplication UseSpaStaticFilesIfNecessary(this WebApplication app, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
            app.UseSpaStaticFiles();
        return app;
    }

    public static IApplicationBuilder UseSpaIfNecessary(this IApplicationBuilder app, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
            app.UseSpa(spaBuilder => spaBuilder.Options.SourcePath = WebClientDirectory);
        return app;
    }
}