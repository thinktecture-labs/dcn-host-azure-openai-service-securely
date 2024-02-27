export type ChatMessage = {
    id: string;
    originator: 'user' | 'ai';
    text: string;
}