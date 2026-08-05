import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Company, Tenant } from '../../../../core/models/phase01.models';

@Component({
  selector: 'app-company-create-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './company-create-modal.component.html',
  styleUrl: './company-create-modal.component.scss'
})
export class CompanyCreateModalComponent implements OnChanges {
  private fb = inject(FormBuilder);

  @Input() isOpen = false;
  @Input() company: Company | null = null;
  @Input() tenants: Tenant[] = [];
  @Input() saving = false;
  @Input() errorMessage = '';
  @Input() defaultTenantId = 0;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  companyForm!: FormGroup;
  submitted = false;

  constructor() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && this.isOpen) {
      this.submitted = false;
      if (this.company) {
        this.companyForm.patchValue({
          tenantId: this.company.tenantId,
          companyCode: this.company.companyCode,
          companyName: this.company.companyName,
          legalName: this.company.legalName || '',
          taxNumber: this.company.taxNumber || '',
          email: this.company.email || '',
          phone: this.company.phone || '',
          website: this.company.website || ''
        });
      } else {
        this.initForm();
        if (this.defaultTenantId) {
          this.companyForm.patchValue({ tenantId: this.defaultTenantId });
        }
      }
    }
  }

  initForm() {
    this.companyForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      companyCode: ['', [Validators.required]],
      companyName: ['', [Validators.required, Validators.maxLength(200)]],
      legalName: [''],
      taxNumber: [''],
      email: ['', [Validators.email]],
      phone: [''],
      website: ['']
    });
  }

  get f() {
    return this.companyForm.controls;
  }

  onClose() {
    this.close.emit();
  }

  onSubmit() {
    this.submitted = true;
    if (this.companyForm.invalid) return;
    this.save.emit(this.companyForm.value);
  }
}
