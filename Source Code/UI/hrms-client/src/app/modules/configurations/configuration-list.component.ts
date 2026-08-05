import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { SystemConfiguration, Tenant } from '../../core/models/phase01.models';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';
import { EmailSettingsComponent } from './email-settings.component';

@Component({
  selector: 'app-configuration-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TruncatePipe, EmailSettingsComponent],
  templateUrl: './configuration-list.component.html',
  styleUrl: './configuration-list.component.scss'
})
export class ConfigurationListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  activeTab: 'system' | 'email' = 'system';

  tenants: Tenant[] = [];
  configurations: SystemConfiguration[] = [];

  tenantIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedConfigId: number | null = null;
  configForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  setTab(tab: 'system' | 'email') {
    this.activeTab = tab;
  }

  initForm() {
    this.configForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      configurationKey: ['', [Validators.required]],
      configurationValue: [''],
      dataType: ['String', [Validators.required]]
    });
  }

  get f() {
    return this.configForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadConfigurations();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadConfigurations() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getConfigurations(this.tenantIdFilter, this.searchText).subscribe({
      next: (res) => {
        this.configurations = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load configurations.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadConfigurations();
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedConfigId = null;
    this.errorMessage = '';
    this.initForm();
    this.configForm.patchValue({
      tenantId: this.tenantIdFilter,
      dataType: 'String'
    });
    this.showModal = true;
  }

  openEditModal(config: SystemConfiguration) {
    this.editMode = true;
    this.submitted = false;
    this.selectedConfigId = config.configurationId;
    this.errorMessage = '';

    this.configForm.patchValue({
      tenantId: config.tenantId,
      configurationKey: config.configurationKey,
      configurationValue: config.configurationValue || '',
      dataType: config.dataType
    });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveConfiguration() {
    this.submitted = true;
    if (this.configForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.configForm.value };

    if (this.editMode && this.selectedConfigId) {
      this.api.updateConfiguration(this.selectedConfigId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Configuration updated.');
            this.closeModal();
            this.loadConfigurations();
          } else {
            this.errorMessage = res.message || 'Failed to update configuration.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createConfiguration(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Configuration variable created.');
            this.closeModal();
            this.loadConfigurations();
          } else {
            this.errorMessage = res.message || 'Failed to create configuration.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    }
  }

  deleteConfiguration(config: SystemConfiguration) {
    if (confirm(`Delete configuration variable: ${config.configurationKey}?`)) {
      this.api.deleteConfiguration(config.configurationId, config.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Configuration deleted.');
            this.loadConfigurations();
          } else {
            this.showToast(res.message || 'Delete failed.');
          }
        },
        error: (err) => {
          console.error(err);
          this.showToast('An error occurred.');
        }
      });
    }
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
