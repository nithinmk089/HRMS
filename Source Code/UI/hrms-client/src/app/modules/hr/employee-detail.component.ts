import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant, Company, BusinessUnit, Department, Designation, Location, CostCenter } from '../../core/models/phase01.models';
import {
  EmployeeDto, EmployeeEmployment, EmployeeAddress, EmployeeContact,
  EmployeeEmergencyContact, EmployeeQualification, EmployeeCertification,
  EmployeeDocument, EmployeeManager, EmployeeTransfer, EmployeePromotion,
  EmployeeServiceHistoryReport
} from '../../core/models/employee.models';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './employee-detail.component.html',
  styleUrl: './employee-detail.component.scss'
})
export class EmployeeDetailComponent implements OnInit {
  private api = inject(ApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  employeeId!: number;
  tenantId!: number;

  employee: EmployeeDto | null = null;
  employment: EmployeeEmployment | null = null;
  addresses: EmployeeAddress[] = [];
  contacts: EmployeeContact[] = [];
  emergencies: EmployeeEmergencyContact[] = [];
  qualifications: EmployeeQualification[] = [];
  certifications: EmployeeCertification[] = [];
  documents: EmployeeDocument[] = [];
  managers: EmployeeManager[] = [];
  directReports: any[] = [];
  serviceHistory: EmployeeServiceHistoryReport[] = [];

  // Dropdown data
  companies: Company[] = [];
  businessUnits: BusinessUnit[] = [];
  departments: Department[] = [];
  designations: Designation[] = [];
  locations: Location[] = [];
  costCenters: CostCenter[] = [];

  // Navigation
  activeTab = 'personal'; // personal, employment, contacts, education, certifications, documents, org, history
  isLoading = false;
  saving = false;
  toastMessage = '';

  // Lifecycle Modals
  showSuspendModal = false;
  showTerminateModal = false;
  showRehireModal = false;
  lifecycleReason = '';

  // CRUD Modals
  showAddressModal = false;
  showContactModal = false;
  showEmergencyModal = false;
  showQualModal = false;
  showCertModal = false;
  showDocModal = false;
  showManagerModal = false;

  addressForm!: FormGroup;
  contactForm!: FormGroup;
  emergencyForm!: FormGroup;
  qualForm!: FormGroup;
  certForm!: FormGroup;
  docForm!: FormGroup;
  managerForm!: FormGroup;

  editMode = false;
  selectedItemId: number | null = null;

  constructor() {
    this.initForms();
  }

  ngOnInit() {
    this.route.params.subscribe(p => {
      this.employeeId = Number(p['id']);
      this.route.queryParams.subscribe(qp => {
        this.tenantId = Number(qp['tenantId']) || 1;
        this.loadAllData();
      });
    });
  }

  initForms() {
    this.addressForm = this.fb.group({
      addressType: ['Current', Validators.required],
      addressLine1: ['', Validators.required],
      addressLine2: [''],
      city: ['', Validators.required],
      state: [''],
      country: ['India', Validators.required],
      zipCode: ['']
    });

    this.contactForm = this.fb.group({
      contactType: ['Mobile', Validators.required],
      contactValue: ['', Validators.required]
    });

    this.emergencyForm = this.fb.group({
      contactName: ['', Validators.required],
      relationship: ['Spouse', Validators.required],
      mobileNumber: ['', Validators.required],
      email: ['', Validators.email]
    });

    this.qualForm = this.fb.group({
      qualificationType: ['Bachelor', Validators.required],
      institution: ['', Validators.required],
      university: [''],
      yearOfPassing: [new Date().getFullYear(), [Validators.required, Validators.min(1900)]],
      percentage: ['']
    });

    this.certForm = this.fb.group({
      certificationName: ['', Validators.required],
      certificationAuthority: ['', Validators.required],
      issueDate: [new Date().toISOString().split('T')[0], Validators.required],
      expiryDate: [''],
      certificateNumber: ['']
    });

    this.docForm = this.fb.group({
      documentType: ['Aadhar Card', Validators.required],
      fileName: ['', Validators.required],
      filePath: ['', Validators.required],
      mimeType: ['application/pdf', Validators.required]
    });

    this.managerForm = this.fb.group({
      managerId: ['', Validators.required],
      effectiveFrom: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  loadAllData() {
    this.isLoading = true;
    // Load employee profile
    this.api.getEmployee(this.employeeId, this.tenantId).subscribe({
      next: (res) => {
        this.employee = res.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load profile.');
      }
    });

    // Load employment details
    this.api.getEmploymentDetails(this.employeeId, this.tenantId).subscribe(res => this.employment = res.data);

    // Load addresses
    this.api.getAddresses(this.employeeId, this.tenantId).subscribe(res => this.addresses = res.data || []);

    // Load contacts
    this.api.getContacts(this.employeeId, this.tenantId).subscribe(res => this.contacts = res.data || []);

    // Load emergencies
    this.api.getEmergencyContacts(this.employeeId, this.tenantId).subscribe(res => this.emergencies = res.data || []);

    // Load qualifications
    this.api.getQualifications(this.employeeId, this.tenantId).subscribe(res => this.qualifications = res.data || []);

    // Load certifications
    this.api.getCertifications(this.employeeId, this.tenantId).subscribe(res => this.certifications = res.data || []);

    // Load documents
    this.api.getDocuments(this.employeeId, this.tenantId).subscribe(res => this.documents = res.data || []);

    // Load managers
    this.api.getManagers(this.employeeId, this.tenantId).subscribe(res => this.managers = res.data || []);

    // Load direct reports
    this.api.getDirectReports(this.employeeId, this.tenantId).subscribe(res => this.directReports = res.data || []);

    // Load service history
    this.api.getServiceHistoryReport(this.employeeId, this.tenantId).subscribe(res => this.serviceHistory = res.data || []);

    // Metadata
    this.api.getCompanies(this.tenantId, undefined, 1, 100).subscribe(res => this.companies = res.data || []);
    this.api.getBusinessUnits(this.tenantId).subscribe(res => this.businessUnits = res.data || []);
    this.api.getDepartments(this.tenantId).subscribe(res => this.departments = res.data || []);
    this.api.getDesignations(this.tenantId).subscribe(res => this.designations = res.data || []);
    this.api.getLocations(this.tenantId).subscribe(res => this.locations = res.data || []);
    this.api.getCostCenters(this.tenantId).subscribe(res => this.costCenters = res.data || []);
  }

  // --- Tab Control ---
  setTab(tab: string) {
    this.activeTab = tab;
  }

  // --- Lifecycle Executions ---

  activate() {
    if (confirm('Activate this employee?')) {
      this.api.activateEmployee(this.employeeId, this.tenantId).subscribe({
        next: () => {
          this.showToast('Employee status set to Active.');
          this.loadAllData();
        },
        error: (err) => console.error(err)
      });
    }
  }

  openSuspend() {
    this.lifecycleReason = '';
    this.showSuspendModal = true;
  }

  confirmSuspend() {
    this.showSuspendModal = false;
    this.api.suspendEmployee(this.employeeId, this.tenantId, this.lifecycleReason).subscribe({
      next: () => {
        this.showToast('Employee status set to Suspended.');
        this.loadAllData();
      },
      error: (err) => console.error(err)
    });
  }

  openTerminate() {
    this.lifecycleReason = '';
    this.showTerminateModal = true;
  }

  confirmTerminate() {
    this.showTerminateModal = false;
    this.api.terminateEmployee(this.employeeId, this.tenantId, this.lifecycleReason).subscribe({
      next: () => {
        this.showToast('Employee status set to Terminated.');
        this.loadAllData();
      },
      error: (err) => console.error(err)
    });
  }

  openRehire() {
    this.lifecycleReason = '';
    this.showRehireModal = true;
  }

  confirmRehire() {
    this.showRehireModal = false;
    this.api.rehireEmployee(this.employeeId, this.tenantId, this.lifecycleReason).subscribe({
      next: () => {
        this.showToast('Employee rehired successfully.');
        this.loadAllData();
      },
      error: (err) => console.error(err)
    });
  }

  // --- Address CRUD ---
  openAddAddress() {
    this.editMode = false;
    this.selectedItemId = null;
    this.addressForm.reset({ addressType: 'Current', country: 'India' });
    this.showAddressModal = true;
  }

  openEditAddress(a: EmployeeAddress) {
    this.editMode = true;
    this.selectedItemId = a.employeeAddressID;
    this.addressForm.patchValue(a);
    this.showAddressModal = true;
  }

  saveAddress() {
    if (this.addressForm.invalid) return;
    const data = { ...this.addressForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    if (this.editMode && this.selectedItemId) {
      this.api.updateAddress(this.employeeId, this.selectedItemId, { ...data, employeeAddressId: this.selectedItemId }).subscribe(() => {
        this.showToast('Address details updated.');
        this.showAddressModal = false;
        this.loadAllData();
      });
    } else {
      this.api.createAddress(this.employeeId, data).subscribe(() => {
        this.showToast('Address added.');
        this.showAddressModal = false;
        this.loadAllData();
      });
    }
  }

  deleteAddress(id: number) {
    if (confirm('Delete address?')) {
      this.api.deleteAddress(this.employeeId, id, this.tenantId).subscribe(() => {
        this.showToast('Address deleted.');
        this.loadAllData();
      });
    }
  }

  // --- Contact CRUD ---
  openAddContact() {
    this.editMode = false;
    this.selectedItemId = null;
    this.contactForm.reset({ contactType: 'Mobile' });
    this.showContactModal = true;
  }

  openEditContact(c: EmployeeContact) {
    this.editMode = true;
    this.selectedItemId = c.employeeContactID;
    this.contactForm.patchValue(c);
    this.showContactModal = true;
  }

  saveContact() {
    if (this.contactForm.invalid) return;
    const data = { ...this.contactForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    if (this.editMode && this.selectedItemId) {
      this.api.updateContact(this.employeeId, this.selectedItemId, { ...data, employeeContactId: this.selectedItemId }).subscribe(() => {
        this.showToast('Contact updated.');
        this.showContactModal = false;
        this.loadAllData();
      });
    } else {
      this.api.createContact(this.employeeId, data).subscribe(() => {
        this.showToast('Contact added.');
        this.showContactModal = false;
        this.loadAllData();
      });
    }
  }

  deleteContact(id: number) {
    if (confirm('Delete contact?')) {
      this.api.deleteContact(this.employeeId, id, this.tenantId).subscribe(() => {
        this.showToast('Contact deleted.');
        this.loadAllData();
      });
    }
  }

  // --- Emergency Contact CRUD ---
  openAddEmergency() {
    this.editMode = false;
    this.selectedItemId = null;
    this.emergencyForm.reset({ relationship: 'Spouse' });
    this.showEmergencyModal = true;
  }

  openEditEmergency(e: EmployeeEmergencyContact) {
    this.editMode = true;
    this.selectedItemId = e.employeeEmergencyContactID;
    this.emergencyForm.patchValue(e);
    this.showEmergencyModal = true;
  }

  saveEmergency() {
    if (this.emergencyForm.invalid) return;
    const data = { ...this.emergencyForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    if (this.editMode && this.selectedItemId) {
      this.api.updateEmergencyContact(this.employeeId, this.selectedItemId, { ...data, employeeEmergencyContactId: this.selectedItemId }).subscribe(() => {
        this.showToast('Emergency contact updated.');
        this.showEmergencyModal = false;
        this.loadAllData();
      });
    } else {
      this.api.createEmergencyContact(this.employeeId, data).subscribe(() => {
        this.showToast('Emergency contact added.');
        this.showEmergencyModal = false;
        this.loadAllData();
      });
    }
  }

  deleteEmergency(id: number) {
    if (confirm('Delete emergency contact?')) {
      this.api.deleteEmergencyContact(this.employeeId, id, this.tenantId).subscribe(() => {
        this.showToast('Emergency contact deleted.');
        this.loadAllData();
      });
    }
  }

  // --- Qualifications CRUD ---
  openAddQual() {
    this.editMode = false;
    this.selectedItemId = null;
    this.qualForm.reset({ qualificationType: 'Bachelor', yearOfPassing: new Date().getFullYear() });
    this.showQualModal = true;
  }

  openEditQual(q: EmployeeQualification) {
    this.editMode = true;
    this.selectedItemId = q.employeeQualificationID;
    this.qualForm.patchValue(q);
    this.showQualModal = true;
  }

  saveQual() {
    if (this.qualForm.invalid) return;
    const data = { ...this.qualForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    if (this.editMode && this.selectedItemId) {
      this.api.updateQualification(this.employeeId, this.selectedItemId, { ...data, employeeQualificationId: this.selectedItemId }).subscribe(() => {
        this.showToast('Qualification details updated.');
        this.showQualModal = false;
        this.loadAllData();
      });
    } else {
      this.api.createQualification(this.employeeId, data).subscribe(() => {
        this.showToast('Qualification added.');
        this.showQualModal = false;
        this.loadAllData();
      });
    }
  }

  deleteQual(id: number) {
    if (confirm('Delete qualification?')) {
      this.api.deleteQualification(this.employeeId, id, this.tenantId).subscribe(() => {
        this.showToast('Qualification deleted.');
        this.loadAllData();
      });
    }
  }

  // --- Certifications CRUD ---
  openAddCert() {
    this.editMode = false;
    this.selectedItemId = null;
    this.certForm.reset({ issueDate: new Date().toISOString().split('T')[0] });
    this.showCertModal = true;
  }

  openEditCert(c: EmployeeCertification) {
    this.editMode = true;
    this.selectedItemId = c.employeeCertificationID;
    this.certForm.patchValue({
      certificationName: c.certificationName,
      certificationAuthority: c.certificationAuthority,
      issueDate: c.issueDate ? new Date(c.issueDate).toISOString().split('T')[0] : '',
      expiryDate: c.expiryDate ? new Date(c.expiryDate).toISOString().split('T')[0] : '',
      certificateNumber: c.certificateNumber || ''
    });
    this.showCertModal = true;
  }

  saveCert() {
    if (this.certForm.invalid) return;
    const data = { ...this.certForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    if (!data.expiryDate) {
      data.expiryDate = null;
    }
    if (this.editMode && this.selectedItemId) {
      this.api.updateCertification(this.employeeId, this.selectedItemId, { ...data, employeeCertificationId: this.selectedItemId }).subscribe(() => {
        this.showToast('Certification details updated.');
        this.showCertModal = false;
        this.loadAllData();
      });
    } else {
      this.api.createCertification(this.employeeId, data).subscribe(() => {
        this.showToast('Certification added.');
        this.showCertModal = false;
        this.loadAllData();
      });
    }
  }

  deleteCert(id: number) {
    if (confirm('Delete certification?')) {
      this.api.deleteCertification(this.employeeId, id, this.tenantId).subscribe(() => {
        this.showToast('Certification deleted.');
        this.loadAllData();
      });
    }
  }

  // --- Documents CRUD ---
  openAddDoc() {
    this.editMode = false;
    this.selectedItemId = null;
    this.docForm.reset({ documentType: 'Aadhar Card', mimeType: 'application/pdf' });
    this.showDocModal = true;
  }

  openEditDoc(d: EmployeeDocument) {
    this.editMode = true;
    this.selectedItemId = d.employeeDocumentID;
    this.docForm.patchValue(d);
    this.showDocModal = true;
  }

  saveDoc() {
    if (this.docForm.invalid) return;
    const data = { ...this.docForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    if (this.editMode && this.selectedItemId) {
      this.api.updateDocument(this.employeeId, this.selectedItemId, { ...data, employeeDocumentId: this.selectedItemId }).subscribe(() => {
        this.showToast('Document updated.');
        this.showDocModal = false;
        this.loadAllData();
      });
    } else {
      this.api.uploadDocument(this.employeeId, data).subscribe(() => {
        this.showToast('Document uploaded successfully.');
        this.showDocModal = false;
        this.loadAllData();
      });
    }
  }

  deleteDoc(id: number) {
    if (confirm('Delete document?')) {
      this.api.deleteDocument(this.employeeId, id, this.tenantId).subscribe(() => {
        this.showToast('Document deleted.');
        this.loadAllData();
      });
    }
  }

  restoreVersion(docId: number, versionNum: number) {
    if (confirm(`Restore document to version ${versionNum}?`)) {
      this.api.restoreDocumentVersion(this.employeeId, docId, versionNum, this.tenantId).subscribe(() => {
        this.showToast(`Document restored to version ${versionNum}.`);
        this.loadAllData();
      });
    }
  }

  // --- Manager Assignment ---
  openAssignManager() {
    this.managerForm.reset({ effectiveFrom: new Date().toISOString().split('T')[0] });
    this.showManagerModal = true;
  }

  saveManager() {
    if (this.managerForm.invalid) return;
    const data = { ...this.managerForm.value, tenantId: this.tenantId, employeeId: this.employeeId };
    this.api.assignManager(this.employeeId, data).subscribe(() => {
      this.showToast('Manager assigned.');
      this.showManagerModal = false;
      this.loadAllData();
    });
  }

  removeManager(managerId: number) {
    if (confirm('Remove manager assignment?')) {
      this.api.removeManager(this.employeeId, managerId, this.tenantId).subscribe(() => {
        this.showToast('Manager assignment removed.');
        this.loadAllData();
      });
    }
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
