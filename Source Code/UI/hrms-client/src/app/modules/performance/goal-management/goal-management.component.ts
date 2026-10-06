import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-goal-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './goal-management.component.html',
  styleUrls: ['./goal-management.component.scss']
})
export class GoalManagementComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  cycles: any[] = [];
  form!: FormGroup;
  progressForm!: FormGroup;
  showModal = false;
  showProgressModal = false;
  selectedGoal: any = null;
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
    this.initProgressForm();
    this.loadData();
    this.loadEmployees();
    this.loadCycles();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', Validators.required],
      performanceCycleId: ['', Validators.required],
      goalTitle: ['', [Validators.required, Validators.maxLength(200)]],
      weightage: [25, [Validators.required, Validators.min(1), Validators.max(100)]],
      targetValue: [100, [Validators.required, Validators.min(0)]],
      goalDescription: ['']
    });
  }

  initProgressForm(): void {
    this.progressForm = this.fb.group({
      progressPercentage: [10, [Validators.required, Validators.min(0), Validators.max(100)]],
      remarks: ['']
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getGoals(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load goals';
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
      performanceCycleId: this.cycles.length > 0 ? this.cycles[0].performanceCycleID : '',
      goalTitle: '',
      weightage: 25,
      targetValue: 100,
      goalDescription: ''
    });
    this.successMessage = '';
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  openProgressModal(goal: any): void {
    this.selectedGoal = goal;
    this.progressForm.reset({
      progressPercentage: goal.achievementPercentage || 0,
      remarks: ''
    });
    this.showProgressModal = true;
  }

  closeProgressModal(): void {
    this.showProgressModal = false;
    this.selectedGoal = null;
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';

    const payload = {
      tenantId: this.tenantId,
      employeeId: Number(this.form.value.employeeId),
      performanceCycleId: Number(this.form.value.performanceCycleId),
      goalTitle: this.form.value.goalTitle,
      weightage: Number(this.form.value.weightage),
      targetValue: Number(this.form.value.targetValue),
      goalDescription: this.form.value.goalDescription
    };

    this.api.createGoal(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Goal established successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to create goal.';
      }
    });
  }

  onSubmitProgress(): void {
    if (this.progressForm.invalid || !this.selectedGoal) return;

    this.isLoading = true;
    const goalId = this.selectedGoal.goalID;
    const payload = {
      tenantId: this.tenantId,
      goalId: goalId,
      progressPercentage: Number(this.progressForm.value.progressPercentage),
      remarks: this.progressForm.value.remarks
    };

    this.api.logGoalProgress(goalId, payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Goal progress updated successfully!';
        this.closeProgressModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to update goal progress.';
      }
    });
  }
}