import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusStyle',
  standalone: true
})
export class StatusPipe implements PipeTransform {
  transform(value: string | undefined): string {
    if (!value) return 'badge-secondary';
    const status = value.trim().toLowerCase();
    switch (status) {
      case 'active':
      case 'enabled':
      case 'true':
        return 'badge-success';
      case 'inactive':
      case 'disabled':
      case 'false':
        return 'badge-danger';
      case 'pending':
      case 'suspended':
        return 'badge-warning';
      default:
        return 'badge-secondary';
    }
  }
}
