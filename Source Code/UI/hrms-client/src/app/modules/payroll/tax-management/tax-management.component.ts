import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-tax-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tax-management.component.html',
  styleUrls: ['./tax-management.component.scss']
})
export class TaxManagementComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  form!: FormGroup;
  showModal = false;
  showViewModal = false;
  selectedItem: any = null;
  successMessage = '';
  errorMessage = '';

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
    // getTaxRegimes returns tax regimes
    this.api.getTaxRegimes(1).subscribe(res => {
      this.items = res.data || [];
    });
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