import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { User, Tenant } from '../../../../core/models/phase01.models';

@Component({
  selector: 'app-user-create-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-create-modal.component.html',
  styleUrl: './user-create-modal.component.scss'
})
export class UserCreateModalComponent implements OnChanges {
  private fb = inject(FormBuilder);

  @Input() isOpen = false;
  @Input() user: User | null = null;
  @Input() tenants: Tenant[] = [];
  @Input() saving = false;
  @Input() errorMessage = '';
  @Input() defaultTenantId = 0;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  userForm!: FormGroup;
  submitted = false;

  constructor() {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && this.isOpen) {
      this.submitted = false;
      this.initForm();
      if (this.user) {
        const u = this.user as any;
        const tenantVal = this.user.tenantId ?? u.tenantID ?? u.tenantId ?? this.defaultTenantId ?? '';
        const empVal = this.user.employeeId ?? u.employeeID ?? u.employeeId ?? '';

        this.userForm.patchValue({
          tenantId: tenantVal,
          userName: this.user.userName,
          email: this.user.email,
          employeeId: empVal
        });
      } else {
        if (this.defaultTenantId) {
          this.userForm.patchValue({ tenantId: this.defaultTenantId });
        }
      }
    }
  }

  initForm() {
    const passwordPattern = '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{12,}$';

    if (this.user) {
      this.userForm = this.fb.group({
        tenantId: ['', [Validators.required]],
        userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
        email: ['', [Validators.required, Validators.email]],
        employeeId: ['']
      });
    } else {
      this.userForm = this.fb.group({
        tenantId: ['', [Validators.required]],
        userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
        email: ['', [Validators.required, Validators.email]],
        employeeId: [''],
        password: ['', [Validators.required, Validators.pattern(passwordPattern)]]
      });
    }
  }

  get f() {
    return this.userForm.controls;
  }

  onClose() {
    this.close.emit();
  }

  onSubmit() {
    this.submitted = true;
    if (this.userForm.invalid) return;
    this.save.emit(this.userForm.value);
  }
}
