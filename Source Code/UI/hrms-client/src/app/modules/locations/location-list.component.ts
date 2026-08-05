import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Location, Tenant } from '../../core/models/phase01.models';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-location-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, TruncatePipe],
  templateUrl: './location-list.component.html',
  styleUrl: './location-list.component.scss'
})
export class LocationListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  locations: Location[] = [];

  tenantIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedLocId: number | null = null;
  locForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.locForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      locationCode: ['', [Validators.required]],
      locationName: ['', [Validators.required]],
      countryCode: [''],
      stateCode: [''],
      city: ['']
    });
  }

  get f() {
    return this.locForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadLocations();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadLocations() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getLocations(this.tenantIdFilter, this.searchText).subscribe({
      next: (res) => {
        this.locations = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load locations.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadLocations();
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedLocId = null;
    this.errorMessage = '';
    this.initForm();
    this.locForm.patchValue({
      tenantId: this.tenantIdFilter
    });
    this.showModal = true;
  }

  openEditModal(loc: Location) {
    this.editMode = true;
    this.submitted = false;
    this.selectedLocId = loc.locationId;
    this.errorMessage = '';

    this.locForm.patchValue({
      tenantId: loc.tenantId,
      locationCode: loc.locationCode,
      locationName: loc.locationName,
      countryCode: loc.countryCode || '',
      stateCode: loc.stateCode || '',
      city: loc.city || ''
    });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveLocation() {
    this.submitted = true;
    if (this.locForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.locForm.value };

    if (this.editMode && this.selectedLocId) {
      this.api.updateLocation(this.selectedLocId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Location updated.');
            this.closeModal();
            this.loadLocations();
          } else {
            this.errorMessage = res.message || 'Failed to update location.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createLocation(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Location created.');
            this.closeModal();
            this.loadLocations();
          } else {
            this.errorMessage = res.message || 'Failed to create location.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    }
  }

  deleteLocation(loc: Location) {
    if (confirm(`Delete location: ${loc.locationName}?`)) {
      this.api.deleteLocation(loc.locationId, loc.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Location deleted.');
            this.loadLocations();
          } else {
            this.showToast(res.message || 'Delete failed.');
          }
        },
        error: (err) => {
          console.error(err);
          this.showToast('An error occurred.');
        }
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
