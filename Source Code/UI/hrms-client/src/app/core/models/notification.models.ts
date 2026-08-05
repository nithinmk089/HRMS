export interface NotificationTemplate {
  templateId: number;
  tenantId: number;
  templateName: string;
  businessEvent: string;
  channel: string;
  subjectTemplate?: string;
  bodyTemplate: string;
  isActive: boolean;
  versionNo: number;
}

export interface NotificationQueue {
  notificationQueueId: number;
  tenantId: number;
  sender?: string;
  recipient: string;
  channel: string;
  businessEvent: string;
  subject?: string;
  body: string;
  status: string;
  errorMessage?: string;
  retryCount: number;
  maxRetries: number;
  nextRunDate: string;
  sentDate?: string;
  deliveredDate?: string;
  readDate?: string;
  escalationStatus: string;
  createdDate: string;
}

export interface UserNotificationPreference {
  preferenceId: number;
  tenantId: number;
  userID: number;
  businessEvent: string;
  channel: string;
  frequency: string;
  isEnabled: boolean;
}

export interface NotificationMetrics {
  totalProcessed: number;
  sentCount: number;
  deliveredCount: number;
  readCount: number;
  failedCount: number;
  deliveryRate: number;
  failureRate: number;
  readRate: number;
}
