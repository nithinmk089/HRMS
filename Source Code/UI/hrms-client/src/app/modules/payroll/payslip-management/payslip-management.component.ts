import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-payslip-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './payslip-management.component.html',
  styleUrls: ['./payslip-management.component.scss']
})
export class PayslipManagementComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  periods: any[] = [];
  form!: FormGroup;
  showModal = false;
  showViewModal = false;
  selectedItem: any = null;
  successMessage = '';
  errorMessage = '';
  searchText = '';
  periodIdFilter: number | null = null;

  constructor(private api: ApiService, private fb: FormBuilder) {}

  viewItem(item: any): void {
    this.selectedItem = item;
    this.showViewModal = true;
  }

  closeViewModal(): void {
    this.showViewModal = false;
    this.selectedItem = null;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadPeriods();
    this.loadData();
    this.loadEmployees();
  }

  loadPeriods(): void {
    this.api.getPayrollPeriods(1, 1).subscribe(res => {
      this.periods = res.data || [];
    });
  }

  initForm(): void {
    this.form = this.fb.group({
      tenantId: [1],
      name: ['', Validators.required],
      code: ['', Validators.required],
      amount: [0, [Validators.required, Validators.min(1)]]
    });
  }

  loadData(): void {
    this.api.getPayslips(1, undefined, this.periodIdFilter || undefined).subscribe(res => {
      let list = res.data || [];
      if (this.searchText) {
        const searchLower = this.searchText.toLowerCase().trim();
        list = list.filter((p: any) =>
          (p.employeeName && p.employeeName.toLowerCase().includes(searchLower)) ||
          (p.employeeCode && p.employeeCode.toLowerCase().includes(searchLower)) ||
          (p.payslipNumber && p.payslipNumber.toLowerCase().includes(searchLower))
        );
      }
      this.items = list;
    });
  }

  resetFilters(): void {
    this.searchText = '';
    this.periodIdFilter = null;
    this.loadData();
  }

  loadEmployees(): void {
    this.api.getEmployees(1).subscribe(res => {
      this.employees = res.data || [];
    });
  }

  openCreateModal(): void {
    this.form.reset({ tenantId: 1, amount: 0 });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.successMessage = 'Operation completed successfully!';
    this.closeModal();
  }
}