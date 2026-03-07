import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-template-image-dialog',
  standalone: true,
  templateUrl: './template-image-dialog.html',
  imports: [CommonModule, DialogModule, ButtonModule],
  styleUrl: './template-image-dialog.css'
})
export class TemplateImageDialog {
  @Input() visible: boolean = false;

  @Input() title: string = '';

  @Input() image: string = '';
  @Output() onClose = new EventEmitter<void>();
  @Output() visibleChange = new EventEmitter<boolean>();

  close() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
    this.onClose.emit();
  }
}
