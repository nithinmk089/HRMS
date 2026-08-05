import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Role, User, Tenant } from '../../core/models/phase01.models';
import { RoleAssignmentModalComponent } from '../../shared/components/modals/role-assignment-modal/role-assignment-modal.component';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-role-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, RoleAssignmentModalComponent, TruncatePipe],
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss'
})
export class RoleListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  roles: Role[] = [];
  
  showMembersModal = false;
  selectedRole: Role | null = null;
  allUsers: User[] = [];
  filteredUsers: User[] = [];
  assignedUserIds: number[] = [];
  userSearchText = '';

  tenantIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedRoleId: number | null = null;
  roleForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.roleForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      roleCode: ['', [Validators.required]],
      roleName: ['', [Validators.required]],
      description: ['']
    });
  }

  get f() {
    return this.roleForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadRoles();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadRoles() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getRoles(this.tenantIdFilter, this.searchText).subscribe({
      next: (res) => {
        this.roles = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load roles.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadRoles();
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedRoleId = null;
    this.errorMessage = '';
    this.initForm();
    this.roleForm.patchValue({
      tenantId: this.tenantIdFilter
    });
    this.showModal = true;
  }

  openEditModal(role: Role) {
    this.editMode = true;
    this.submitted = false;
    this.selectedRoleId = role.roleId;
    this.errorMessage = '';

    this.roleForm.patchValue({
      tenantId: role.tenantId,
      roleCode: role.roleCode,
      roleName: role.roleName,
      description: role.description || ''
    });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveRole() {
    this.submitted = true;
    if (this.roleForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.roleForm.value };

    if (this.editMode && this.selectedRoleId) {
      this.api.updateRole(this.selectedRoleId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Role details updated.');
            this.closeModal();
            this.loadRoles();
          } else {
            this.errorMessage = res.message || 'Failed to update role.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createRole(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Role created successfully.');
            this.closeModal();
            this.loadRoles();
          } else {
            this.errorMessage = res.message || 'Failed to create role.';
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

  deleteRole(role: Role) {
    if (confirm(`Delete role profile: ${role.roleName}?`)) {
      this.api.deleteRole(role.roleId, role.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Role deleted.');
            this.loadRoles();
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

  openMembersModal(role: Role) {
    this.selectedRole = role;
    this.showMembersModal = true;
  }

  closeMembersModal() {
    this.showMembersModal = false;
    this.selectedRole = null;
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
