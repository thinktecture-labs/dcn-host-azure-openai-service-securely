using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostingAzureOpenAiSample.WebApp.OpenAiAccess;

public static class OpenAiAccessModule
{
    public static IServiceCollection AddOpenAiAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var options = OpenAiAccessOptions.FromConfiguration(configuration);
        var openAiClient = OpenAiClientFactory.CreateOpenAiClient(options);
        return services.AddSingleton(options)
                       .AddSingleton(openAiClient);
    }
}