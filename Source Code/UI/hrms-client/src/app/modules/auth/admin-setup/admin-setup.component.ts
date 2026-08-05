import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-setup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-setup.component.html',
  styleUrl: './admin-setup.component.scss'
})
export class AdminSetupComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  setupForm: FormGroup = this.fb.group({
    // Step 1: Tenant Info
    tenantCode: ['TEN-ACME-001', [Validators.required, Validators.pattern('^[a-zA-Z0-9-_.]+$')]],
    tenantName: ['Acme Enterprise Group', [Validators.required]],

    // Step 2: Primary Company Info
    companyCode: ['COMP-ACME-TECH', [Validators.required, Validators.pattern('^[a-zA-Z0-9-_.]+$')]],
    companyName: ['Acme Technology Corp', [Validators.required]],
    legalName: ['Acme Technology Corp LLC'],
    taxNumber: ['TAX-US-9988001'],
    companyEmail: ['contact@acmetech.com', [Validators.email]],
    companyPhone: ['+1-555-0101'],

    // Step 3: Admin Employee & User Account Details
    employeeCode: ['EMP-001', [Validators.required]],
    firstName: ['System', [Validators.required]],
    lastName: ['Administrator', [Validators.required]],
    adminEmail: ['admin@hrms.com', [Validators.required, Validators.email]],
    password: ['admin123', [Validators.required, Validators.minLength(6)]],
    mobileNumber: ['+1-555-0199']
  });

  currentStep = 1;
  isLoading = false;
  errorMessage = '';

  ngOnInit() {
    // Verify setup is needed
    this.authService.checkSetupStatus().subscribe({
      next: (res: any) => {
        if (res.data && !res.data.needsSetup) {
          // Setup already completed, go to login
          this.router.navigate(['/login']);
        }
      }
    });
  }

  nextStep() {
    this.errorMessage = '';
    if (this.currentStep === 1) {
      const tc = this.setupForm.get('tenantCode');
      const tn = this.setupForm.get('tenantName');
      tc?.markAsTouched();
      tn?.markAsTouched();
      if (tc?.invalid || tn?.invalid) {
        this.errorMessage = 'Please fix validation errors in Step 1 before continuing.';
        return;
      }
    } else if (this.currentStep === 2) {
      const cc = this.setupForm.get('companyCode');
      const cn = this.setupForm.get('companyName');
      cc?.markAsTouched();
      cn?.markAsTouched();
      if (cc?.invalid || cn?.invalid) {
        this.errorMessage = 'Please fix validation errors in Step 2 before continuing.';
        return;
      }
    }
    this.currentStep++;
  }

  prevStep() {
    this.errorMessage = '';
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  onSubmit() {
    if (this.setupForm.invalid) {
      this.setupForm.markAllAsTouched();
      this.errorMessage = 'Please fill all required fields properly.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.setupInitialAdmin(this.setupForm.value).subscribe({
      next: (res: any) => {
        this.isLoading = false;
        if (res.success) {
          this.router.navigate(['/dashboard']);
        } else {
          this.errorMessage = res.message || 'Setup failed. Please try again.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Server connection error during setup.';
      }
    });
  }
}
