using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostingAzureOpenAiSample.WebApp.CompositionRoot;

public static class Cors
{
    public static IServiceCollection AddCorsIfNecessary(this IServiceCollection services, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
            return services;

        return services.AddCors(
            options => options.AddDefaultPolicy(
                p => p.AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithOrigins("http://localhost:4200")
            )
        );
    }

    public static WebApplication UseCorsIfNecessary(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseCors();
        return app;
    }
}