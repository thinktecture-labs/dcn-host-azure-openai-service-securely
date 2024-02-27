using System.Collections.Generic;

namespace HostingAzureOpenAiSample.WebApp.Chat;

public sealed class ChatDto
{
    private List<ChatMessage>? _messages;

    public List<ChatMessage> Messages
    {
        get => _messages ??= [];
        set => _messages = value;
    }
}