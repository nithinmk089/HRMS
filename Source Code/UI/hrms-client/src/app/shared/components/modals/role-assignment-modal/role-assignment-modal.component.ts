import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../../core/services/api.service';
import { Role, User } from '../../../../core/models/phase01.models';

@Component({
  selector: 'app-role-assignment-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './role-assignment-modal.component.html',
  styleUrl: './role-assignment-modal.component.scss'
})
export class RoleAssignmentModalComponent implements OnChanges {
  private api = inject(ApiService);

  @Input() isOpen = false;
  @Input() role: Role | null = null;

  @Output() close = new EventEmitter<void>();
  @Output() notify = new EventEmitter<string>();

  allUsers: User[] = [];
  filteredUsers: User[] = [];
  assignedUserIds: number[] = [];
  userSearchText = '';
  isLoading = false;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && this.isOpen && this.role) {
      this.loadUsersAndAssignments();
    }
  }

  loadUsersAndAssignments() {
    if (!this.role) return;
    this.isLoading = true;
    this.userSearchText = '';
    
    this.api.getUsers(this.role.tenantId).subscribe({
      next: (res) => {
        this.allUsers = res.data || [];
        this.filteredUsers = [...this.allUsers];
        // Note: For now, in mock API, we can fetch role assignments or start with empty assignment array
        this.assignedUserIds = [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.notify.emit('Failed to load users.');
      }
    });
  }

  filterUsers() {
    if (!this.userSearchText) {
      this.filteredUsers = [...this.allUsers];
    } else {
      const q = this.userSearchText.toLowerCase();
      this.filteredUsers = this.allUsers.filter(u => 
        u.userName.toLowerCase().includes(q) || u.email.toLowerCase().includes(q)
      );
    }
  }

  isUserAssigned(userId: number): boolean {
    return this.assignedUserIds.includes(userId);
  }

  toggleUserRoleMembership(userId: number, event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked;
    if (!this.role) return;

    const action = isChecked 
      ? this.api.assignRole(this.role.roleId, userId, this.role.tenantId)
      : this.api.removeRole(this.role.roleId, userId, this.role.tenantId);

    action.subscribe({
      next: (res) => {
        if (res.success) {
          if (isChecked) {
            this.assignedUserIds.push(userId);
            this.notify.emit('User assigned to role.');
          } else {
            this.assignedUserIds = this.assignedUserIds.filter(id => id !== userId);
            this.notify.emit('User removed from role.');
          }
        } else {
          this.notify.emit(res.message || 'Membership toggle failed.');
          (event.target as HTMLInputElement).checked = !isChecked;
        }
      },
      error: (err) => {
        console.error(err);
        this.notify.emit('An error occurred during assignment.');
        (event.target as HTMLInputElement).checked = !isChecked;
      }
    });
  }

  onClose() {
    this.close.emit();
  }
}
