import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { ExitStatusReportDto, ClearanceStatusReportDto, FullAndFinalSummaryReportDto } from '../../core/models/onboarding-offboarding.models';

@Component({
  selector: 'app-offboarding-workflow',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './offboarding-workflow.component.html',
  styleUrl: './offboarding-workflow.component.scss'
})
export class OffboardingWorkflowComponent implements OnInit {
  private apiService = inject(ApiService);
  private fb = inject(FormBuilder);

  tenantId = 1;
  isLoading = false;

  exitRequests: ExitStatusReportDto[] = [];
  clearanceRequests: ClearanceStatusReportDto[] = [];
  settlements: FullAndFinalSummaryReportDto[] = [];

  exitForm!: FormGroup;
  settlementForm!: FormGroup;
  
  showExitModal = false;
  showSettlementModal = false;
  selectedEmployeeIdForSettlement: number | null = null;

  ngOnInit(): void {
    this.tenantId = Number(localStorage.getItem('tenantId') || '1');
    this.initForms();
    this.loadData();
  }

  initForms(): void {
    this.exitForm = this.fb.group({
      employeeID: ['', [Validators.required]],
      resignationDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      lastWorkingDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      exitReason: ['', [Validators.required]]
    });

    this.settlementForm = this.fb.group({
      settlementAmount: [0, [Validators.required, Validators.min(0)]]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.apiService.getExitStatusReport(this.tenantId).subscribe({
      next: (res) => {
        this.exitRequests = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading exit status report', err);
        this.isLoading = false;
      }
    });

    this.apiService.getClearanceStatusReport(this.tenantId).subscribe({
      next: (res) => {
        this.clearanceRequests = res.data || [];
      },
      error: (err) => console.error('Error loading clearance status report', err)
    });

    this.apiService.getFullAndFinalSummaryReport(this.tenantId).subscribe({
      next: (res) => {
        this.settlements = res.data || [];
      },
      error: (err) => console.error('Error loading settlement summary report', err)
    });
  }

  onSubmitExit(): void {
    if (this.exitForm.invalid) return;

    const body = {
      tenantID: this.tenantId,
      ...this.exitForm.value
    };

    this.apiService.submitExitRequest(body).subscribe({
      next: () => {
        this.showExitModal = false;
        this.exitForm.reset();
        this.loadData();
      },
      error: (err) => console.error('Error submitting exit request', err)
    });
  }

  approveExit(id: number): void {
    this.apiService.approveExitRequest(id, { exitRequestID: id, tenantID: this.tenantId, remarks: 'Resignation Approved' }).subscribe({
      next: () => this.loadData(),
      error: (err) => console.error('Error approving exit request', err)
    });
  }

  rejectExit(id: number): void {
    this.apiService.rejectExitRequest(id, { exitRequestID: id, tenantID: this.tenantId, remarks: 'Resignation Rejected' }).subscribe({
      next: () => this.loadData(),
      error: (err) => console.error('Error rejecting exit request', err)
    });
  }

  initiateClearance(employeeId: number): void {
    const body = {
      tenantID: this.tenantId,
      employeeID: employeeId,
      initiatedDate: new Date().toISOString().split('T')[0]
    };

    this.apiService.initiateClearance(body).subscribe({
      next: () => this.loadData(),
      error: (err) => console.error('Error initiating clearance', err)
    });
  }

  openSettlementModal(employeeId: number): void {
    this.selectedEmployeeIdForSettlement = employeeId;
    this.showSettlementModal = true;
  }

  onCalculateSettlement(): void {
    if (this.settlementForm.invalid || this.selectedEmployeeIdForSettlement === null) return;

    const body = {
      tenantID: this.tenantId,
      employeeID: this.selectedEmployeeIdForSettlement,
      settlementAmount: this.settlementForm.value.settlementAmount
    };

    this.apiService.calculateFullAndFinal(body).subscribe({
      next: () => {
        this.showSettlementModal = false;
        this.settlementForm.reset();
        this.selectedEmployeeIdForSettlement = null;
        this.loadData();
      },
      error: (err) => console.error('Error calculating FFS', err)
    });
  }

  getApprovedCount(): number {
    return this.exitRequests.filter(r => r.status === 'Approved').length;
  }
}
