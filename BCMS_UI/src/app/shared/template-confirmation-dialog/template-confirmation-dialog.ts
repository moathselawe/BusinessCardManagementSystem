import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-template-confirmation-dialog',
  standalone: true,
  templateUrl: './template-confirmation-dialog.html',
  imports: [CommonModule, DialogModule, ButtonModule],
  styleUrl: './template-confirmation-dialog.css'
})  
export class TemplateConfirmationDialog {
  @Input() visible: boolean = false;
  @Input() message: string = '';

  @Output() onConfirm = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();
  @Output() visibleChange = new EventEmitter<boolean>();


  onCancelClicked() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
    this.onCancel.emit();
  }

  onConfirmClicked() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
    this.onConfirm.emit();
  }

}
