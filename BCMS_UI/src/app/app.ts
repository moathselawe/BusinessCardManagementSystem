import { Component, signal } from '@angular/core';
import { ChatMessage } from './shared/template-conversation/template-conversation';
import { ToastMessageService } from './services/shared/toast-message.service';
import { CdkDragEnd } from '@angular/cdk/drag-drop';
import { AIService } from './services/chatbot.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrls: ['./app.css']
})
export class App {
  constructor(
    private service: AIService,
    private toastService: ToastMessageService
  ) { }
  protected readonly title = signal('BCMS_UI');
  visible = false;

  aiMessages: ChatMessage[] = [
    { sender: 'bot', text: 'Hello! How can I assist you today?' }
  ];

  onAiMessage(msg: string) {
    if (!msg?.trim()) return;

    this.service.chatbot({ message: msg }).subscribe({
      next: (res: any) => {
        const botText = res?.response?.reply || 'Sorry, I could not process your request.';
        this.aiMessages.push({
          sender: 'bot',
          text: botText,
          isLongText: botText.length > 100
        });
      },
      error: (err) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to get response from AI.'
        });
        console.error('failed', err);

        this.aiMessages.push({
          sender: 'bot',
          text: 'Sorry, something went wrong. Please try again later.'
        });
      }
    });
  }
}
