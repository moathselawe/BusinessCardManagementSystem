import { Component, signal } from '@angular/core';
import { ChatMessage } from './shared/template-conversation/template-conversation';
import { ToastMessageService } from './services/shared/toast-message.service';
import { AIService } from './services/shared/aiService.service';

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
  protected readonly title = signal('HireMind_UI');
  visible = false;
  isDragging = false;

  aiMessages: ChatMessage[] = [ 
    { sender: 'bot', text: 'Hello! How can I assist you today?' }
  ];

  handleClick() {
    if (this.isDragging) return; // 👈 block click after drag

    this.visible = !this.visible;
  }

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
