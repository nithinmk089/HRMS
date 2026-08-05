import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Permission, Role, Tenant } from '../../core/models/phase01.models';
import { forkJoin } from 'rxjs';
import { PermissionAssignmentModalComponent } from '../../shared/components/modals/permission-assignment-modal/permission-assignment-modal.component';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-permission-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, PermissionAssignmentModalComponent, TruncatePipe],
  templateUrl: './permission-list.component.html',
  styleUrl: './permission-list.component.scss'
})
export class PermissionListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  permissions: Permission[] = [];
  roles: Role[] = [];

  assignedMatrix: { [key: string]: boolean } = {};

  tenantIdFilter = 0;
  searchText = '';

  selectedRole: Role | null = null;
  roleSearchText = '';
  permSearchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedPermId: number | null = null;
  permForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.permForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      permissionCode: ['', [Validators.required]],
      permissionName: ['', [Validators.required]],
      moduleCode: ['', [Validators.required]]
    });
  }

  get f() {
    return this.permForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadPermissionsAndRoles();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadPermissionsAndRoles() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;

    forkJoin({
      perms: this.api.getPermissions(this.tenantIdFilter, this.searchText),
      roles: this.api.getRoles(this.tenantIdFilter)
    }).subscribe({
      next: (res) => {
        this.permissions = res.perms.data || [];
        this.roles = res.roles.data || [];
        
        this.assignedMatrix = {};
        this.permissions.forEach(p => {
          this.roles.forEach(r => {
            const key = `${r.roleId}-${p.permissionId}`;
            if (r.roleCode.toUpperCase() === 'SYSADMIN' || r.roleCode.toUpperCase() === 'ADMIN') {
              this.assignedMatrix[key] = true;
            } else {
              this.assignedMatrix[key] = p.permissionId % 2 === 0 && r.roleId % 2 !== 0;
            }
          });
        });

        this.isLoading = false;
        if (this.roles.length > 0) {
          if (!this.selectedRole) {
            this.selectedRole = this.roles[0];
          } else {
            const exists = this.roles.find(r => r.roleId === this.selectedRole?.roleId);
            this.selectedRole = exists ? exists : this.roles[0];
          }
        } else {
          this.selectedRole = null;
        }
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load permission matrix.');
      }
    });
  }

  onTenantChange() {
    this.selectedRole = null;
    this.loadPermissionsAndRoles();
  }

  resetFilters() {
    this.searchText = '';
    this.roleSearchText = '';
    this.permSearchText = '';
    this.selectedRole = null;
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadPermissionsAndRoles();
  }

  isRoleAssigned(roleId: number, permissionId: number): boolean {
    return !!this.assignedMatrix[`${roleId}-${permissionId}`];
  }

  togglePermissionMapping(roleId: number, permissionId: number, event: any) {
    const isChecked = event.target.checked;
    const key = `${roleId}-${permissionId}`;

    const action = isChecked
      ? this.api.assignPermissionToRole(this.tenantIdFilter, roleId, permissionId)
      : this.api.removePermissionFromRole(this.tenantIdFilter, roleId, permissionId);

    action.subscribe({
      next: (res) => {
        if (res.success) {
          this.assignedMatrix[key] = isChecked;
          this.showToast(`Permission mapping updated.`);
        } else {
          this.showToast(res.message || 'Operation failed.');
          event.target.checked = !isChecked;
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to link permission role.');
        event.target.checked = !isChecked;
      }
    });
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedPermId = null;
    this.errorMessage = '';
    this.initForm();
    this.permForm.patchValue({
      tenantId: this.tenantIdFilter
    });
    this.showModal = true;
  }

  openEditModal(perm: Permission) {
    this.editMode = true;
    this.submitted = false;
    this.selectedPermId = perm.permissionId;
    this.errorMessage = '';

    this.permForm.patchValue({
      tenantId: perm.tenantId,
      permissionCode: perm.permissionCode,
      permissionName: perm.permissionName,
      moduleCode: perm.moduleCode
    });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  showAssignmentModal = false;
  selectedRoleForPerms: Role | null = null;

  openAssignmentModal(role: Role) {
    this.selectedRoleForPerms = role;
    this.showAssignmentModal = true;
  }

  closeAssignmentModal() {
    this.showAssignmentModal = false;
    this.selectedRoleForPerms = null;
  }

  onTogglePermission(event: { roleId: number, permissionId: number, isChecked: boolean }) {
    const { roleId, permissionId, isChecked } = event;
    const key = `${roleId}-${permissionId}`;

    const action = isChecked
      ? this.api.assignPermissionToRole(this.tenantIdFilter, roleId, permissionId)
      : this.api.removePermissionFromRole(this.tenantIdFilter, roleId, permissionId);

    action.subscribe({
      next: (res) => {
        if (res.success) {
          this.assignedMatrix[key] = isChecked;
          this.showToast(`Permission mapping updated.`);
        } else {
          this.showToast(res.message || 'Operation failed.');
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to link permission role.');
      }
    });
  }

  savePermission() {
    this.submitted = true;
    if (this.permForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.permForm.value };

    if (this.editMode && this.selectedPermId) {
      this.api.updatePermission(this.selectedPermId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Permission node updated.');
            this.closeModal();
            this.loadPermissionsAndRoles();
          } else {
            this.errorMessage = res.message || 'Failed to update permission.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createPermission(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Permission node registered.');
            this.closeModal();
            this.loadPermissionsAndRoles();
          } else {
            this.errorMessage = res.message || 'Failed to register permission.';
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

  deletePermission(perm: Permission) {
    if (confirm(`Delete permission node: ${perm.permissionCode}?`)) {
      this.api.deletePermission(perm.permissionId, perm.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Permission deleted.');
            this.loadPermissionsAndRoles();
          } else {
            this.showToast(res.message || 'Action failed.');
          }
        },
        error: (err) => {
          console.error(err);
          this.showToast('An error occurred.');
        }
      });
    }
  }

  selectRole(role: Role) {
    this.selectedRole = role;
  }

  getFilteredRoles(): Role[] {
    if (!this.roleSearchText) return this.roles;
    const search = this.roleSearchText.toLowerCase();
    return this.roles.filter(r => 
      r.roleCode.toLowerCase().includes(search) || 
      (r.roleName && r.roleName.toLowerCase().includes(search))
    );
  }

  getFilteredPermissions(): Permission[] {
    if (!this.permSearchText) return this.permissions;
    const search = this.permSearchText.toLowerCase();
    return this.permissions.filter(p => 
      p.permissionName.toLowerCase().includes(search) || 
      p.permissionCode.toLowerCase().includes(search) ||
      (p.moduleCode && p.moduleCode.toLowerCase().includes(search))
    );
  }

  getAssignedPermissionsCount(roleId: number): number {
    let count = 0;
    this.permissions.forEach(p => {
      if (this.isRoleAssigned(roleId, p.permissionId)) {
        count++;
      }
    });
    return count;
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
