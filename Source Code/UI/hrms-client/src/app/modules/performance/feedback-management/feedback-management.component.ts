import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-feedback-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './feedback-management.component.html',
  styleUrls: ['./feedback-management.component.scss']
})
export class FeedbackManagementComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';
  isLoading = false;
  tenantId = 1;

  constructor(
    private api: ApiService,
    private auth: AuthService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.tenantId = this.auth.getTenantId() || 1;
    this.initForm();
    this.loadData();
    this.loadEmployees();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', Validators.required],
      feedbackText: ['', [Validators.required, Validators.maxLength(2000)]]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getContinuousFeedback(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load feedback';
        this.isLoading = false;
      }
    });
  }

  loadEmployees(): void {
    this.api.getEmployees(this.tenantId).subscribe({
      next: (res) => {
        this.employees = res.data || [];
      },
      error: (err) => console.error('Failed to load employees', err)
    });
  }

  openCreateModal(): void {
    this.form.reset({
      employeeId: this.employees.length > 0 ? this.employees[0].employeeId : '',
      feedbackText: ''
    });
    this.successMessage = '';
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';

    const payload = {
      tenantId: this.tenantId,
      employeeId: Number(this.form.value.employeeId),
      feedbackText: this.form.value.feedbackText
    };

    this.api.createContinuousFeedback(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Continuous feedback recorded successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to record feedback.';
      }
    });
  }
}