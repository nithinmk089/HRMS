import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface AuditLogItem {
  id?: number;
  activity: string;
  performedBy: string;
  timestamp: string | Date;
  ipAddress?: string;
  details?: string;
}

@Component({
  selector: 'app-audit-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './audit-history.component.html',
  styleUrl: './audit-history.component.scss'
})
export class AuditHistoryComponent {
  @Input() auditLogs: AuditLogItem[] = [];
  @Input() isLoading: boolean = false;
}
