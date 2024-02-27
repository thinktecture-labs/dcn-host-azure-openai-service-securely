using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using HostingAzureOpenAiSample.WebApp.OpenAiAccess;
using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace HostingAzureOpenAiSample.WebApp.Chat;

public static class ChatEndpoint
{
    public static WebApplication MapOpenAiChat(this WebApplication app)
    {
        app.MapPost("/api/chat", StreamChatResponse);
        return app;
    }

    private static async Task StreamChatResponse(OpenAIClient openAiClient,
                                                 OpenAiAccessOptions openAiAccessOptions,
                                                 HttpResponse response,
                                                 ChatDto dto,
                                                 ILogger logger,
                                                 CancellationToken cancellationToken)
    {
        response.StatusCode = StatusCodes.Status200OK;
        response.ContentType = "text/plain";
        var messages = new List<ChatRequestMessage>(dto.Messages.Count + 1)
        {
            new ChatRequestSystemMessage(
                "Please keep your answers short, they should be two paragraphs long or shorter.")
        };
        messages.AddRange(
            dto.Messages.Select(
                m => m.Originator == Originators.Ai ?
                    (ChatRequestMessage) new ChatRequestAssistantMessage(m.Text) :
                    new ChatRequestUserMessage(m.Text)
            )
        );

        var options = new ChatCompletionsOptions(openAiAccessOptions.ModelName, messages)
        {
            ChoiceCount = 1,
            MaxTokens = 1000
        };
        var streamingResponse = await openAiClient.GetChatCompletionsStreamingAsync(options, cancellationToken);
        await foreach (var item in streamingResponse)
        {
            logger.Debug("{@StreamItem}", item);
            if (!item.ContentUpdate.IsNullOrEmpty()) // Do not ignore white space responses
                await response.WriteAsync(item.ContentUpdate, cancellationToken);
        }
    }
}