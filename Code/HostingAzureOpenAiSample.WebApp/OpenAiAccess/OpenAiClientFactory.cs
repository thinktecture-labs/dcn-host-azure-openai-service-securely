using System;
using Azure;
using Azure.AI.OpenAI;

namespace HostingAzureOpenAiSample.WebApp.OpenAiAccess;

public static class OpenAiClientFactory
{
    public static OpenAIClient CreateOpenAiClient(OpenAiAccessOptions options) =>
        options.Type switch
        {
            OpenAiServiceType.OpenAi => new (options.ApiKey),
            OpenAiServiceType.AzureOpenAi => new (new Uri(options.AzureOpenAiEndpoint),
                                                  new AzureKeyCredential(options.ApiKey)),
            _ => throw new ArgumentException($"{options.Type} is unknown", nameof(options))
        };
}