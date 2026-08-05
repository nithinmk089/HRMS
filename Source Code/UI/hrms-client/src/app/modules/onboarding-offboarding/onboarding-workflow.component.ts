import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { OnboardingStatusReportDto, OnboardingWorkflowDto, PendingTasksReportDto } from '../../core/models/onboarding-offboarding.models';

@Component({
  selector: 'app-onboarding-workflow',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './onboarding-workflow.component.html',
  styleUrl: './onboarding-workflow.component.scss'
})
export class OnboardingWorkflowComponent implements OnInit {
  private apiService = inject(ApiService);
  private fb = inject(FormBuilder);

  tenantId = 1;
  isLoading = false;
  
  workflows: OnboardingWorkflowDto[] = [];
  statusReports: OnboardingStatusReportDto[] = [];
  pendingTasks: PendingTasksReportDto[] = [];
  
  workflowForm!: FormGroup;
  showCreateModal = false;

  ngOnInit(): void {
    this.tenantId = Number(localStorage.getItem('tenantId') || '1');
    this.initForm();
    this.loadData();
  }

  initForm(): void {
    this.workflowForm = this.fb.group({
      employeeID: ['', [Validators.required]],
      workflowCode: ['', [Validators.required]],
      workflowName: ['', [Validators.required]],
      startDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      targetCompletionDate: [new Date().toISOString().split('T')[0], [Validators.required]]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.apiService.getOnboardingStatusReport(this.tenantId).subscribe({
      next: (res) => {
        this.statusReports = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading onboarding status report', err);
        this.isLoading = false;
      }
    });

    this.apiService.getPendingOnboardingTasksReport(this.tenantId).subscribe({
      next: (res) => {
        this.pendingTasks = res.data || [];
      },
      error: (err) => console.error('Error loading pending tasks report', err)
    });
  }

  getCompletedCount(): number {
    return this.statusReports.filter(r => r.workflowStatus === 'Completed').length;
  }

  onCreateWorkflow(): void {
    if (this.workflowForm.invalid) return;
    
    const body = {
      tenantID: this.tenantId,
      ...this.workflowForm.value
    };

    this.apiService.createOnboardingWorkflow(body).subscribe({
      next: (res) => {
        this.showCreateModal = false;
        this.workflowForm.reset();
        this.loadData();
      },
      error: (err) => console.error('Error creating onboarding workflow', err)
    });
  }

  startWorkflow(id: number): void {
    this.apiService.startOnboardingWorkflow(id, this.tenantId).subscribe({
      next: () => this.loadData(),
      error: (err) => console.error('Error starting workflow', err)
    });
  }

  completeWorkflow(id: number): void {
    this.apiService.completeOnboardingWorkflow(id, this.tenantId).subscribe({
      next: () => this.loadData(),
      error: (err) => console.error('Error completing workflow', err)
    });
  }

  completeTask(assignmentId: number): void {
    this.apiService.completeOnboardingTaskAssignment(assignmentId, this.tenantId).subscribe({
      next: () => this.loadData(),
      error: (err) => console.error('Error completing task assignment', err)
    });
  }
}
