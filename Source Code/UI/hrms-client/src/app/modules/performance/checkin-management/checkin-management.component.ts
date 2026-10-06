import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-checkin-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkin-management.component.html',
  styleUrls: ['./checkin-management.component.scss']
})
export class CheckinManagementComponent implements OnInit {
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
    const today = new Date().toISOString().split('T')[0];
    this.form = this.fb.group({
      employeeId: ['', Validators.required],
      managerId: ['', Validators.required],
      meetingDate: [today, Validators.required],
      notes: ['']
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getCheckIns(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load 1-on-1 check-ins';
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
    const today = new Date().toISOString().split('T')[0];
    this.form.reset({
      employeeId: this.employees.length > 0 ? this.employees[0].employeeId : '',
      managerId: this.employees.length > 1 ? this.employees[1].employeeId : (this.employees[0]?.employeeId || ''),
      meetingDate: today,
      notes: ''
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
      managerId: Number(this.form.value.managerId),
      meetingDate: this.form.value.meetingDate,
      notes: this.form.value.notes
    };

    this.api.createCheckIn(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || '1-on-1 Check-in scheduled successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to schedule check-in.';
      }
    });
  }
}