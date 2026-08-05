import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../core/services/notification.service';
import { NotificationTemplate } from '../../core/models/notification.models';

@Component({
  selector: 'app-notification-templates',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './notification-templates.component.html',
  styleUrl: './notification-templates.component.scss'
})
export class NotificationTemplatesComponent implements OnInit {
  private notificationService = inject(NotificationService);

  templates: NotificationTemplate[] = [];
  isLoading = true;
  tenantId = 1;
  searchText = '';

  // Form Modal/Drawer toggle state
  isEditing = false;
  selectedTemplate: Partial<NotificationTemplate> = {};

  businessEvents = ['Tenant Created', 'Subscription Expiring', 'Payment Failed', 'License Threshold Reached'];
  channels = ['In-App', 'Email', 'SMS', 'Push', 'Teams'];
  toastMessage = '';

  ngOnInit() {
    this.tenantId = Number(localStorage.getItem('tenantId') || '1');
    this.loadTemplates();
  }

  loadTemplates() {
    this.isLoading = true;
    this.notificationService.getTemplates(this.tenantId, this.searchText || undefined)
      .subscribe({
        next: (res) => {
          this.templates = res.data || [];
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to load templates', err);
          this.isLoading = false;
          this.showToast('Failed to load notification templates.');
        }
      });
  }

  search() {
    this.loadTemplates();
  }

  resetSearch() {
    this.searchText = '';
    this.loadTemplates();
  }

  addNewTemplate() {
    this.selectedTemplate = {
      tenantId: this.tenantId,
      templateName: '',
      businessEvent: this.businessEvents[0],
      channel: this.channels[0],
      subjectTemplate: '',
      bodyTemplate: '',
      isActive: true,
      versionNo: 1
    };
    this.isEditing = true;
  }

  editTemplate(template: NotificationTemplate) {
    this.selectedTemplate = { ...template };
    this.isEditing = true;
  }

  saveTemplate() {
    if (!this.selectedTemplate.templateName || !this.selectedTemplate.bodyTemplate) {
      this.showToast('Please fill in all required fields.');
      return;
    }

    if (this.selectedTemplate.templateId) {
      // Update
      this.notificationService.updateTemplate(this.selectedTemplate.templateId, this.selectedTemplate)
        .subscribe({
          next: (res) => {
            if (res.success) {
              this.showToast('Template updated successfully.');
              this.isEditing = false;
              this.loadTemplates();
            } else {
              this.showToast(res.message || 'Update failed.');
            }
          },
          error: (err) => {
            console.error(err);
            this.showToast('An error occurred during update.');
          }
        });
    } else {
      // Create
      this.notificationService.createTemplate(this.selectedTemplate)
        .subscribe({
          next: (res) => {
            if (res.success) {
              this.showToast('Template created successfully.');
              this.isEditing = false;
              this.loadTemplates();
            } else {
              this.showToast(res.message || 'Creation failed.');
            }
          },
          error: (err) => {
            console.error(err);
            this.showToast('An error occurred during template creation.');
          }
        });
    }
  }

  deleteTemplate(id: number) {
    if (confirm('Are you sure you want to delete this notification template?')) {
      this.notificationService.deleteTemplate(id, this.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Template deleted successfully.');
            this.loadTemplates();
          } else {
            this.showToast(res.message || 'Deletion failed.');
          }
        },
        error: (err) => {
          console.error(err);
          this.showToast('An error occurred.');
        }
      });
    }
  }

  cancelEdit() {
    this.isEditing = false;
    this.selectedTemplate = {};
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
