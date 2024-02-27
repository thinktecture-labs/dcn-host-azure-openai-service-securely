import { Routes } from '@angular/router';
import { AiChatComponent } from './ai-chat/ai-chat.component';

export const routes: Routes = [
    {
        path: 'chat',
        component: AiChatComponent,
        title: 'Host OpenAI Securly',
    },
    { path: '', redirectTo: '/chat', pathMatch: 'full'}
];
