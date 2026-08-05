import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-payroll-run',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './payroll-run.component.html',
  styleUrls: ['./payroll-run.component.scss']
})
export class PayrollRunComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  form!: FormGroup;
  showModal = false;
  showViewModal = false;
  selectedItem: any = null;
  successMessage = '';
  errorMessage = '';
  searchText = '';
  statusFilter = '';

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
    this.loadData();
    this.loadEmployees();
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
    this.api.getPayrollRuns(1).subscribe(res => {
      let list = res.data || [];
      if (this.statusFilter) {
        list = list.filter((r: any) => r.runStatus === this.statusFilter);
      }
      if (this.searchText) {
        const searchLower = this.searchText.toLowerCase().trim();
        list = list.filter((r: any) =>
          (r.periodCode && r.periodCode.toLowerCase().includes(searchLower)) ||
          (r.runStatus && r.runStatus.toLowerCase().includes(searchLower))
        );
      }
      this.items = list;
    });
  }

  resetFilters(): void {
    this.searchText = '';
    this.statusFilter = '';
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