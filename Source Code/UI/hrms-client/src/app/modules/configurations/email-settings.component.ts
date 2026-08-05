import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant } from '../../core/models/phase01.models';

@Component({
  selector: 'app-email-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './email-settings.component.html',
  styleUrl: './email-settings.component.scss'
})
export class EmailSettingsComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  selectedTenantId = 1;

  emailForm!: FormGroup;
  isLoading = false;
  saving = false;
  sendingTest = false;

  testRecipient = '';
  showTestModal = false;
  toastMessage = '';
  errorMessage = '';
  testResult: any = null;

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.emailForm = this.fb.group({
      mode: ['Development', [Validators.required]],
      devRecipient: ['admin@hrms.com', [Validators.required, Validators.email]],
      smtpHost: ['smtp.mailtrap.io', [Validators.required]],
      smtpPort: [587, [Validators.required, Validators.min(1)]],
      smtpUsername: [''],
      smtpPassword: [''],
      enableSsl: [true],
      fromAddress: ['noreply@hrms.com', [Validators.required, Validators.email]],
      fromName: ['Enterprise HRMS System', [Validators.required]]
    });
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.selectedTenantId = this.tenants[0].tenantId;
          this.loadEmailSettings();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants list.');
      }
    });
  }

  onTenantChange() {
    this.loadEmailSettings();
  }

  loadEmailSettings() {
    if (!this.selectedTenantId) return;
    this.isLoading = true;
    this.errorMessage = '';
    this.api.getEmailSettings(this.selectedTenantId).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.success && res.data) {
          const s = res.data;
          this.emailForm.patchValue({
            mode: s.mode || 'Development',
            devRecipient: s.devRecipient || 'admin@hrms.com',
            smtpHost: s.smtpHost || 'smtp.mailtrap.io',
            smtpPort: s.smtpPort || 587,
            smtpUsername: s.smtpUsername || '',
            smtpPassword: s.smtpPassword || '',
            enableSsl: s.enableSsl !== false,
            fromAddress: s.fromAddress || 'noreply@hrms.com',
            fromName: s.fromName || 'Enterprise HRMS System'
          });
          this.testRecipient = s.devRecipient || 'admin@hrms.com';
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);
        this.errorMessage = 'Failed to load email settings.';
      }
    });
  }

  saveSettings() {
    if (this.emailForm.invalid) {
      this.emailForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    const payload = this.emailForm.value;

    this.api.saveEmailSettings(this.selectedTenantId, payload).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast(`Email settings successfully saved for tenant (Mode: ${payload.mode}).`);
        } else {
          this.errorMessage = res.message || 'Failed to save email settings.';
        }
      },
      error: (err) => {
        this.saving = false;
        console.error(err);
        this.errorMessage = 'An error occurred while saving email settings.';
      }
    });
  }

  openTestModal() {
    this.showTestModal = true;
    this.testResult = null;
    if (!this.testRecipient) {
      this.testRecipient = this.emailForm.value.devRecipient || 'admin@hrms.com';
    }
  }

  closeTestModal() {
    this.showTestModal = false;
  }

  sendTestEmail() {
    if (!this.testRecipient) return;

    this.sendingTest = true;
    this.testResult = null;

    this.api.sendTestEmail(this.selectedTenantId, this.testRecipient).subscribe({
      next: (res) => {
        this.sendingTest = false;
        if (res.success && res.data) {
          this.testResult = res.data;
          this.showToast('Test email dispatched successfully!');
        } else {
          this.testResult = { success: false, errorMessage: res.message || 'Test email failed.' };
        }
      },
      error: (err) => {
        this.sendingTest = false;
        console.error(err);
        this.testResult = { success: false, errorMessage: 'An error occurred during test email dispatch.' };
      }
    });
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 4000);
  }
}
