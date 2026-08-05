import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Organization {
  organizationId: number;
  organizationCode: string;
  organizationName: string;
  industry?: string;
  website?: string;
  logoPath?: string;
  statusName?: string;
  createdDate?: Date;
}

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/organizations`;

  searchOrganizations(searchText?: string, page: number = 1, pageSize: number = 20): Observable<Organization[]> {
    let params: any = { page, pageSize };
    if (searchText) {
      params.searchText = searchText;
    }
    return this.http.get<Organization[]>(this.apiUrl, { params });
  }

  getOrganization(id: number): Observable<Organization> {
    return this.http.get<Organization>(`${this.apiUrl}/${id}`);
  }

  createOrganization(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }
}
