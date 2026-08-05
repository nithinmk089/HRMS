import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';

@Directive({
  selector: '[appPermission]',
  standalone: true
})
export class PermissionDirective {
  private templateRef = inject(TemplateRef);
  private viewContainer = inject(ViewContainerRef);

  @Input() set appPermission(permission: string | string[]) {
    const permissions: string[] = JSON.parse(localStorage.getItem('permissions') || '[]');
    const isSysAdmin = permissions.includes('SYSADMIN');

    let hasPermission = false;
    if (isSysAdmin) {
      hasPermission = true;
    } else if (Array.isArray(permission)) {
      hasPermission = permission.some(p => permissions.includes(p));
    } else {
      hasPermission = permissions.includes(permission);
    }

    if (hasPermission) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
