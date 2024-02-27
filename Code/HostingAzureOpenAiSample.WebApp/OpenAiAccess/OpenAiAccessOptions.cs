using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using Microsoft.Extensions.Configuration;

namespace HostingAzureOpenAiSample.WebApp.OpenAiAccess;

public sealed class OpenAiAccessOptions
{
    public const string DefaultSectionName = "OpenAiAccess";

    public OpenAiServiceType Type { get; set; }
    public string AzureOpenAiEndpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;

    public static OpenAiAccessOptions FromConfiguration(
        IConfiguration configuration,
        string sectionName = DefaultSectionName
    )
    {
        sectionName.MustNotBeNullOrWhiteSpace();
        var options = new OpenAiAccessOptions();
        var section = configuration.GetSection(sectionName);
        if (!section.Exists())
            throw new InvalidConfigurationException($"The section \"{sectionName}\" is missing from the configuration");
        section.Bind(options);

        options.ApiKey.MustNotBeNullOrWhiteSpace();
        options.ModelName.MustNotBeNullOrWhiteSpace();
        if (options.Type == OpenAiServiceType.AzureOpenAi)
            options.AzureOpenAiEndpoint.MustNotBeNullOrWhiteSpace();
        return options;
    }
}