import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiService } from '../../../../core/services/api.service';
import { Department, BusinessUnit, Tenant } from '../../../../core/models/phase01.models';

@Component({
  selector: 'app-department-create-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './department-create-modal.component.html',
  styleUrl: './department-create-modal.component.scss'
})
export class DepartmentCreateModalComponent implements OnChanges {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);

  @Input() isOpen = false;
  @Input() department: Department | null = null;
  @Input() tenants: Tenant[] = [];
  @Input() saving = false;
  @Input() errorMessage = '';
  @Input() defaultTenantId = 0;
  @Input() defaultBusinessUnitId = 0;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  deptForm!: FormGroup;
  submitted = false;

  businessUnits: BusinessUnit[] = [];
  potentialParents: Department[] = [];

  constructor() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && this.isOpen) {
      this.submitted = false;
      if (this.department) {
        this.loadFormContext(this.department.tenantId, this.department.departmentId);
        this.deptForm.patchValue({
          tenantId: this.department.tenantId,
          businessUnitId: this.department.businessUnitId,
          parentDepartmentId: this.department.parentDepartmentId || '',
          departmentCode: this.department.departmentCode,
          departmentName: this.department.departmentName
        });
      } else {
        this.initForm();
        if (this.defaultTenantId) {
          this.deptForm.patchValue({ tenantId: this.defaultTenantId });
          this.loadFormContext(this.defaultTenantId, null);
        }
      }
    }
  }

  initForm() {
    this.deptForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      businessUnitId: ['', [Validators.required]],
      parentDepartmentId: [''],
      departmentCode: ['', [Validators.required]],
      departmentName: ['', [Validators.required]]
    });
  }

  get f() {
    return this.deptForm.controls;
  }

  loadFormContext(tenantId: number, excludeDeptId: number | null) {
    this.api.getBusinessUnits(tenantId, undefined, undefined).subscribe({
      next: (res) => {
        this.businessUnits = res.data || [];
        if (!this.department && this.defaultBusinessUnitId && this.businessUnits.some(b => b.businessUnitId === this.defaultBusinessUnitId)) {
          this.deptForm.patchValue({ businessUnitId: this.defaultBusinessUnitId });
        } else if (!this.department && this.businessUnits.length > 0) {
          this.deptForm.patchValue({ businessUnitId: this.businessUnits[0].businessUnitId });
        }
      }
    });

    this.api.getDepartments(tenantId, undefined, undefined).subscribe({
      next: (res) => {
        this.potentialParents = (res.data || []).filter(d => d.departmentId !== excludeDeptId);
      }
    });
  }

  onTenantChange() {
    const tid = Number(this.deptForm.value.tenantId);
    if (tid) {
      this.loadFormContext(tid, this.department ? this.department.departmentId : null);
    } else {
      this.businessUnits = [];
      this.potentialParents = [];
    }
  }

  onClose() {
    this.close.emit();
  }

  onSubmit() {
    this.submitted = true;
    if (this.deptForm.invalid) return;
    this.save.emit(this.deptForm.value);
  }
}
