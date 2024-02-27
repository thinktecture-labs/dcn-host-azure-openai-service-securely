import { Component, ViewChild } from '@angular/core';
import { ChatInputComponent } from "./chat-input/chat-input.component";
import { ChatMessagesComponent } from "./chat-messages/chat-messages.component";
import { ChatMessage } from './chat-messages/chat-message';
import { ChatService } from './chat.service';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
    selector: 'app-ai-chat',
    standalone: true,
    templateUrl: './ai-chat.component.html',
    styleUrl: './ai-chat.component.scss',
    imports: [MatToolbarModule, ChatInputComponent, ChatMessagesComponent]
})
export class AiChatComponent {

    @ViewChild('chatMessages') chatMessages!: ChatMessagesComponent;
    @ViewChild('chatInput') chatInput!: ChatInputComponent;

    constructor(private readonly chatService: ChatService) { }

    async onMessageSubmitted(message: string) {
        this.chatInput.disableInput();
        const newUserMessage: ChatMessage = { id: crypto.randomUUID(), originator: 'user', text: message };
        this.chatMessages.addMessage(newUserMessage);
        this.chatService
            .getAiResponse(this.chatMessages.finishedMessages)
            .subscribe(
                {
                    next: response => {
                        if (response.isFinsihed) {
                            const aiMessage: ChatMessage = { id: crypto.randomUUID(), originator: 'ai', text: response.message };
                            this.chatMessages.addMessage(aiMessage);
                        }
                        else {
                            this.chatMessages.updateStreamedMessage(response.message);
                        }
                    },
                    complete: () => {
                        this.chatInput.enableInput();
                    }
                }
            );
    }

}
