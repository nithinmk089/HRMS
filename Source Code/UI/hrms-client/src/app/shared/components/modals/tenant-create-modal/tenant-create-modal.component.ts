import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Tenant } from '../../../../core/models/phase01.models';

@Component({
  selector: 'app-tenant-create-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tenant-create-modal.component.html',
  styleUrl: './tenant-create-modal.component.scss'
})
export class TenantCreateModalComponent implements OnChanges {
  private fb = inject(FormBuilder);

  @Input() isOpen = false;
  @Input() tenant: Tenant | null = null;
  @Input() saving = false;
  @Input() errorMessage = '';

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  tenantForm!: FormGroup;
  submitted = false;

  constructor() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && this.isOpen) {
      this.submitted = false;
      if (this.tenant) {
        const fromDate = this.tenant.effectiveFrom ? this.tenant.effectiveFrom.substring(0, 10) : '';
        const toDate = this.tenant.effectiveTo ? this.tenant.effectiveTo.substring(0, 10) : '';
        this.tenantForm.patchValue({
          tenantCode: this.tenant.tenantCode,
          tenantName: this.tenant.tenantName,
          effectiveFrom: fromDate,
          effectiveTo: toDate,
          status: this.tenant.status
        });
      } else {
        this.initForm();
      }
    }
  }

  initForm() {
    this.tenantForm = this.fb.group({
      tenantCode: ['', [Validators.required]],
      tenantName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      effectiveFrom: [new Date().toISOString().substring(0, 10), [Validators.required]],
      effectiveTo: [''],
      status: ['Active']
    });
  }

  get f() {
    return this.tenantForm.controls;
  }

  onClose() {
    this.close.emit();
  }

  onSubmit() {
    this.submitted = true;
    if (this.tenantForm.invalid) return;
    this.save.emit(this.tenantForm.value);
  }
}
