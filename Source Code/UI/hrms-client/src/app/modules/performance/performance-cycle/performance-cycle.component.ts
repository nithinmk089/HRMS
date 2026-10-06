import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-performance-cycle',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './performance-cycle.component.html',
  styleUrls: ['./performance-cycle.component.scss']
})
export class PerformanceCycleComponent implements OnInit {
  items: any[] = [];
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
  }

  initForm(): void {
    const currentYear = new Date().getFullYear();
    this.form = this.fb.group({
      cycleCode: [`CYC-${currentYear}`, [Validators.required, Validators.maxLength(50)]],
      cycleName: [`Annual Appraisal ${currentYear}`, [Validators.required, Validators.maxLength(100)]],
      startDate: [`${currentYear}-01-01`, Validators.required],
      endDate: [`${currentYear}-12-31`, Validators.required]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getPerformanceCycles(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load performance cycles';
        this.isLoading = false;
      }
    });
  }

  openCreateModal(): void {
    const currentYear = new Date().getFullYear();
    this.form.reset({
      cycleCode: `CYC-${currentYear}-${Math.floor(Math.random() * 900 + 100)}`,
      cycleName: `Appraisal Cycle ${currentYear}`,
      startDate: `${currentYear}-01-01`,
      endDate: `${currentYear}-12-31`
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
      cycleCode: this.form.value.cycleCode,
      cycleName: this.form.value.cycleName,
      startDate: this.form.value.startDate,
      endDate: this.form.value.endDate
    };

    this.api.createPerformanceCycle(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Performance appraisal cycle created successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to create performance cycle.';
      }
    });
  }

  openCycle(cycle: any): void {
    if (!cycle?.performanceCycleID) return;
    this.isLoading = true;
    this.api.openPerformanceCycle(cycle.performanceCycleID, this.tenantId).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = `Cycle "${cycle.cycleName}" opened for appraisals!`;
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to open cycle.';
      }
    });
  }
}