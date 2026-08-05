import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Permission, Role } from '../../../../core/models/phase01.models';

@Component({
  selector: 'app-permission-assignment-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './permission-assignment-modal.component.html',
  styleUrl: './permission-assignment-modal.component.scss'
})
export class PermissionAssignmentModalComponent {
  @Input() isOpen = false;
  @Input() role: Role | null = null;
  @Input() permissions: Permission[] = [];
  @Input() assignedMatrix: { [key: string]: boolean } = {};

  @Output() close = new EventEmitter<void>();
  @Output() toggle = new EventEmitter<{ roleId: number, permissionId: number, isChecked: boolean }>();

  isPermissionAssigned(permissionId: number): boolean {
    if (!this.role) return false;
    return !!this.assignedMatrix[`${this.role.roleId}-${permissionId}`];
  }

  onToggle(permissionId: number, event: Event) {
    if (!this.role) return;
    const isChecked = (event.target as HTMLInputElement).checked;
    this.toggle.emit({ roleId: this.role.roleId, permissionId, isChecked });
  }

  onClose() {
    this.close.emit();
  }
}
