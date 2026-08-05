import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-final-settlement',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './final-settlement.component.html',
  styleUrls: ['./final-settlement.component.scss']
})
export class FinalSettlementComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

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
    // API load simulator
    this.api.getEmployeeCompensations(1).subscribe(res => {
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