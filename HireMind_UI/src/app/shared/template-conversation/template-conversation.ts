import { Component, Input, Output, EventEmitter, ElementRef, ViewChild, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToolbarModule } from 'primeng/toolbar';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { MessageModule } from 'primeng/message';

export interface ChatMessage {
  sender: 'user' | 'bot';
  text: string;
  isLongText?: boolean;
}

@Component({
  selector: 'app-template-conversation',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ToolbarModule,
    CardModule,
    InputTextModule,
    ButtonModule,
    ScrollPanelModule,
    MessageModule
  ],
  templateUrl: './template-conversation.html',
  styleUrls: ['./template-conversation.css']
})
export class TemplateConversation {
  @Input() title: string = 'Chatbot Assistant';
  @Input() messages: ChatMessage[] = [];
  @Output() closed = new EventEmitter<void>();
  @Output() messageSent = new EventEmitter<string>();

  @ViewChild('chatBody') private chatBody!: ElementRef<HTMLDivElement>;
  message: string = '';

  sendMessage() {
    if (!this.message?.trim()) return;

    this.messageSent.emit(this.message);

    this.messages.push({ sender: 'user', text: this.message });

    this.message = '';
    this.scrollToBottom();
  }

  // Whenever messages update (after view checked)
  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    try {
      this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
    } catch (err) { }
  }
}
