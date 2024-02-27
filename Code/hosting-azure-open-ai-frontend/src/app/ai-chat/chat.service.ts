import { HttpClient, HttpDownloadProgressEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, filter, map } from 'rxjs';
import { ChatMessage } from './chat-messages/chat-message';
import { environment } from '../../environments/environment';

type ChatDto = {
  messages: ChatMessage[];
}

export type AiResponseDto = {
  message: string;
  isFinsihed: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private readonly httpClient: HttpClient) { }

  getAiResponse(chatMessages: ChatMessage[]): Observable<AiResponseDto> {

    const url = environment.apiBaseAddress + '/api/chat';
    const dto: ChatDto = { messages: chatMessages };
    return this.httpClient
      .post(
        url,
        dto,
        {
          reportProgress: true,
          observe: 'events',
          responseType: 'text'
        }
      )
      .pipe(
        filter(event => event.type === HttpEventType.Response || event.type === HttpEventType.DownloadProgress),
        map(event => {
          let message: string = '';
          let isFinsihed: boolean = false;
          if (event.type === HttpEventType.DownloadProgress) {
            const progressEvent = event as HttpDownloadProgressEvent;
            if (progressEvent.partialText) {
              message = progressEvent.partialText;
            }
          }
          else if (event.type === HttpEventType.Response) {
            isFinsihed = true;
            if (event.body) {
              message = event.body;
            }
          }

          const responseDto: AiResponseDto = {
            message,
            isFinsihed
          };
          return responseDto;
        })
      );
  }
}
