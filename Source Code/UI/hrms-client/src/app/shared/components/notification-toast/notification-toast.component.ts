import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface ToastMessage {
  id: number;
  message: string;
  type: 'success' | 'danger' | 'warning' | 'info';
}

@Component({
  selector: 'app-notification-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification-toast.component.html',
  styleUrl: './notification-toast.component.scss'
})
export class NotificationToastComponent {
  @Input() toasts: ToastMessage[] = [];
  @Output() close = new EventEmitter<number>();

  onClose(id: number) {
    this.close.emit(id);
  }
}
