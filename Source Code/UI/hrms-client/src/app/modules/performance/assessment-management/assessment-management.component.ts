import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-assessment-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './assessment-management.component.html',
  styleUrls: ['./assessment-management.component.scss']
})
export class AssessmentManagementComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  cycles: any[] = [];
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
    this.loadCycles();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', Validators.required],
      performanceCycleId: ['', Validators.required]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getSelfAssessments(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load self-assessments';
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

  loadCycles(): void {
    this.api.getPerformanceCycles(this.tenantId).subscribe({
      next: (res) => {
        this.cycles = res.data || [];
      },
      error: (err) => console.error('Failed to load cycles', err)
    });
  }

  openCreateModal(): void {
    this.form.reset({
      employeeId: this.employees.length > 0 ? this.employees[0].employeeId : '',
      performanceCycleId: this.cycles.length > 0 ? this.cycles[0].performanceCycleID : ''
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
      performanceCycleId: Number(this.form.value.performanceCycleId)
    };

    this.api.createSelfAssessment(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Self assessment record generated successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to create self assessment.';
      }
    });
  }
}