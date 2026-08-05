import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'userDisplay',
  standalone: true
})
export class UserDisplayPipe implements PipeTransform {
  transform(value: any): string {
    if (!value) return '';
    if (typeof value === 'string') return value;
    
    const firstName = value.firstName || '';
    const lastName = value.lastName || '';
    const email = value.email || '';
    const userName = value.userName || '';

    const fullName = `${firstName} ${lastName}`.trim();
    if (fullName) {
      return email ? `${fullName} (${email})` : fullName;
    }
    
    if (userName) {
      return email ? `${userName} (${email})` : userName;
    }

    return email || 'Unknown User';
  }
}
