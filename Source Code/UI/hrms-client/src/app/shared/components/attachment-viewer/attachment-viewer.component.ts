import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface AttachmentItem {
  id?: number | string;
  name: string;
  url: string;
  fileType: string; // e.g. 'image/png', 'application/pdf', etc.
  size?: number; // size in bytes
}

@Component({
  selector: 'app-attachment-viewer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './attachment-viewer.component.html',
  styleUrl: './attachment-viewer.component.scss'
})
export class AttachmentViewerComponent {
  @Input() attachments: AttachmentItem[] = [];
  @Input() allowDelete: boolean = false;
  
  @Output() delete = new EventEmitter<AttachmentItem>();

  selectedAttachment: AttachmentItem | null = null;

  isImage(type: string): boolean {
    return type.startsWith('image/');
  }

  formatSize(bytes?: number): string {
    if (!bytes) return '';
    const kb = bytes / 1024;
    if (kb < 1024) return `${kb.toFixed(1)} KB`;
    return `${(kb / 1024).toFixed(1)} MB`;
  }

  viewAttachment(item: AttachmentItem) {
    this.selectedAttachment = item;
  }

  closePreview() {
    this.selectedAttachment = null;
  }

  onDelete(item: AttachmentItem, event: Event) {
    event.stopPropagation();
    this.delete.emit(item);
  }
}
