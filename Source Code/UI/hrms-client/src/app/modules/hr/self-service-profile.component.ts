import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import {
  EmployeeDto, EmployeeEmployment, EmployeeAddress, EmployeeContact,
  EmployeeEmergencyContact, EmployeeQualification, EmployeeCertification,
  EmployeeDocument, EmployeeServiceHistoryReport
} from '../../core/models/employee.models';

@Component({
  selector: 'app-self-service-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './self-service-profile.component.html',
  styleUrl: './self-service-profile.component.scss'
})
export class SelfServiceProfileComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  // Simulated logged-in employee (SYSADMIN/Nithin is employee ID 1, tenant ID 1)
  employeeId = 1;
  tenantId = 1;

  employee: EmployeeDto | null = null;
  employment: EmployeeEmployment | null = null;
  addresses: EmployeeAddress[] = [];
  contacts: EmployeeContact[] = [];
  emergencies: EmployeeEmergencyContact[] = [];
  qualifications: EmployeeQualification[] = [];
  certifications: EmployeeCertification[] = [];
  documents: EmployeeDocument[] = [];
  serviceHistory: EmployeeServiceHistoryReport[] = [];

  activeTab = 'profile'; // profile, documents, history
  isLoading = false;
  saving = false;
  toastMessage = '';

  // Edit Forms
  showProfileEditModal = false;
  showContactEditModal = false;
  showDocUploadModal = false;

  profileForm!: FormGroup;
  contactForm!: FormGroup;
  docForm!: FormGroup;

  errorMessage = '';

  constructor() {
    this.initForms();
  }

  ngOnInit() {
    this.loadSelfData();
  }

  initForms() {
    this.profileForm = this.fb.group({
      preferredName: ['', Validators.maxLength(100)],
      maritalStatus: ['Single', Validators.required]
    });

    this.contactForm = this.fb.group({
      personalEmail: ['', [Validators.required, Validators.email]],
      mobileNumber: ['', [Validators.required, Validators.maxLength(20)]]
    });

    this.docForm = this.fb.group({
      documentType: ['Passport', Validators.required],
      fileName: ['', Validators.required],
      filePath: ['', Validators.required],
      mimeType: ['application/pdf', Validators.required]
    });
  }

  loadSelfData() {
    this.isLoading = true;
    this.api.getEmployee(this.employeeId, this.tenantId).subscribe({
      next: (res) => {
        this.employee = res.data;
        if (this.employee) {
          this.profileForm.patchValue({
            preferredName: this.employee.preferredName || '',
            maritalStatus: this.employee.maritalStatus || 'Single'
          });
          this.contactForm.patchValue({
            personalEmail: this.employee.personalEmail || '',
            mobileNumber: this.employee.mobileNumber || ''
          });
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load profile data.');
      }
    });

    this.api.getEmploymentDetails(this.employeeId, this.tenantId).subscribe(res => this.employment = res.data);
    this.api.getAddresses(this.employeeId, this.tenantId).subscribe(res => this.addresses = res.data || []);
    this.api.getContacts(this.employeeId, this.tenantId).subscribe(res => this.contacts = res.data || []);
    this.api.getEmergencyContacts(this.employeeId, this.tenantId).subscribe(res => this.emergencies = res.data || []);
    this.api.getQualifications(this.employeeId, this.tenantId).subscribe(res => this.qualifications = res.data || []);
    this.api.getCertifications(this.employeeId, this.tenantId).subscribe(res => this.certifications = res.data || []);
    this.api.getDocuments(this.employeeId, this.tenantId).subscribe(res => this.documents = res.data || []);
    this.api.getServiceHistoryReport(this.employeeId, this.tenantId).subscribe(res => this.serviceHistory = res.data || []);
  }

  setTab(tab: string) {
    this.activeTab = tab;
  }

  openProfileEdit() {
    this.errorMessage = '';
    this.showProfileEditModal = true;
  }

  saveProfile() {
    if (this.profileForm.invalid) return;
    this.saving = true;
    const body = { ...this.profileForm.value, tenantId: this.tenantId };
    this.api.updateEmployeeProfile(this.employeeId, this.tenantId, body.preferredName, body.maritalStatus).subscribe({
      next: () => {
        this.saving = false;
        this.showToast('Personal profile updated.');
        this.showProfileEditModal = false;
        this.loadSelfData();
      },
      error: (err) => {
        this.saving = false;
        console.error(err);
        this.errorMessage = 'Failed to save profile.';
      }
    });
  }

  openContactEdit() {
    this.errorMessage = '';
    this.showContactEditModal = true;
  }

  saveContacts() {
    if (this.contactForm.invalid) return;
    this.saving = true;
    const body = { ...this.contactForm.value, tenantId: this.tenantId };
    this.api.updateEmployeeContactDetails(this.employeeId, this.tenantId, body.personalEmail, body.mobileNumber).subscribe({
      next: () => {
        this.saving = false;
        this.showToast('Contact details updated.');
        this.showContactEditModal = false;
        this.loadSelfData();
      },
      error: (err) => {
        this.saving = false;
        console.error(err);
        this.errorMessage = 'Failed to save contacts.';
      }
    });
  }

  openDocUpload() {
    this.errorMessage = '';
    this.docForm.reset({ documentType: 'Passport', mimeType: 'application/pdf' });
    this.showDocUploadModal = true;
  }

  uploadDoc() {
    if (this.docForm.invalid) return;
    this.saving = true;
    const data = { ...this.docForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    this.api.uploadDocument(this.employeeId, data).subscribe({
      next: () => {
        this.saving = false;
        this.showToast('Document uploaded successfully.');
        this.showDocUploadModal = false;
        this.loadSelfData();
      },
      error: (err) => {
        this.saving = false;
        console.error(err);
        this.errorMessage = 'Failed to upload document.';
      }
    });
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
