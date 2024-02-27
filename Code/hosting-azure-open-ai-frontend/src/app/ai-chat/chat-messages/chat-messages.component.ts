import { Component } from '@angular/core';
import { ChatMessage } from './chat-message';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MarkdownComponent } from 'ngx-markdown';

@Component({
  selector: 'app-chat-messages',
  standalone: true,
  imports: [MatIconModule, MatProgressBarModule, MarkdownComponent],
  templateUrl: './chat-messages.component.html',
  styleUrl: './chat-messages.component.scss'
})
export class ChatMessagesComponent {

  readonly finishedMessages: ChatMessage[] = [];
  currentlyStreamedMessage?: string;

  addMessage(message: ChatMessage) {
    this.finishedMessages.push(message);
    if (message.originator === 'ai') {
      this.currentlyStreamedMessage = undefined;
    }
  }

  updateStreamedMessage(message: string) {
    this.currentlyStreamedMessage = message;
  }

}
