import { Component, OnInit, inject } from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { User, Tenant } from '../../core/models/phase01.models';
import { UserCreateModalComponent } from '../../shared/components/modals/user-create-modal/user-create-modal.component';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, DatePipe, NgClass, UserCreateModalComponent, TruncatePipe],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  users: User[] = [];

  tenantIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedUserId: number | null = null;
  selectedUser: User | null = null;
  userForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    const passwordPattern = '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{12,}$';

    this.userForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      employeeId: [''],
      password: ['', [Validators.required, Validators.pattern(passwordPattern)]]
    });
  }

  get f() {
    return this.userForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadUsers();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadUsers() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getUsers(this.tenantIdFilter, this.searchText).subscribe({
      next: (res) => {
        this.users = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load users.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadUsers();
  }

  openCreateModal() {
    this.editMode = false;
    this.selectedUserId = null;
    this.selectedUser = null;
    this.errorMessage = '';
    this.showModal = true;
  }

  openEditModal(user: User) {
    this.editMode = true;
    this.selectedUserId = user.userId;
    this.selectedUser = user;
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedUser = null;
  }

  onSaveUser(formData: any) {
    this.saving = true;
    this.errorMessage = '';

    if (formData.employeeId === '') {
      formData.employeeId = null;
    } else {
      formData.employeeId = Number(formData.employeeId);
    }

    if (this.editMode && this.selectedUserId) {
      this.api.updateUser(this.selectedUserId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('User profile updated.');
            this.closeModal();
            this.loadUsers();
          } else {
            this.errorMessage = res.message || 'Failed to update user.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createUser(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('User created successfully.');
            this.closeModal();
            this.loadUsers();
          } else {
            this.errorMessage = res.message || 'Failed to create user.';
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

  toggleLock(user: User, lock: boolean) {
    const action = lock ? this.api.lockUser(user.userId, user.tenantId) : this.api.unlockUser(user.userId, user.tenantId);
    action.subscribe({
      next: (res) => {
        if (res.success) {
          this.showToast(`User account ${lock ? 'locked' : 'unlocked'} successfully.`);
          this.loadUsers();
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

  deleteUser(user: User) {
    if (confirm(`Delete user account: ${user.userName}?`)) {
      this.api.deleteUser(user.userId, user.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('User account deleted.');
            this.loadUsers();
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
