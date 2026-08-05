import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-employee-compensation',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './employee-compensation.component.html',
  styleUrls: ['./employee-compensation.component.scss']
})
export class EmployeeCompensationComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  structures: any[] = [];
  form!: FormGroup;
  showModal = false;
  showViewModal = false;
  selectedItem: any = null;
  successMessage = '';
  errorMessage = '';
  searchText = '';
  structureIdFilter: number | null = null;

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
    this.loadStructures();
    this.loadData();
    this.loadEmployees();
  }

  loadStructures(): void {
    this.api.getSalaryStructures(1).subscribe(res => {
      this.structures = res.data || [];
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
    this.api.getEmployeeCompensations(1).subscribe(res => {
      let list = res.data || [];
      if (this.structureIdFilter) {
        list = list.filter((c: any) => c.salaryStructureID === Number(this.structureIdFilter));
      }
      if (this.searchText) {
        const searchLower = this.searchText.toLowerCase().trim();
        list = list.filter((c: any) =>
          (c.employeeName && c.employeeName.toLowerCase().includes(searchLower)) ||
          (c.employeeCode && c.employeeCode.toLowerCase().includes(searchLower))
        );
      }
      this.items = list;
    });
  }

  resetFilters(): void {
    this.searchText = '';
    this.structureIdFilter = null;
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