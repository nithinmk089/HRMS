import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { FormBuilder } from '@angular/forms';

import { CompanyListComponent } from './company-list.component';
import { ApiService } from '../../core/services/api.service';
import { ApiResponse } from '../../core/models/phase01.models';

describe('CompanyListComponent', () => {
  let component: CompanyListComponent;
  let fixture: ComponentFixture<CompanyListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', ['getTenants', 'getCompanies', 'createCompany', 'updateCompany', 'deleteCompany']);

    await TestBed.configureTestingModule({
      imports: [CompanyListComponent],
      providers: [
        FormBuilder,
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should load tenants and companies on init', () => {
    const tenantsResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [{ tenantId: 1, tenantCode: 'T1', tenantName: 'Tenant 1', status: 'Active' }]
    };
    const companiesResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [{ companyId: 1, companyCode: 'C1', companyName: 'Company 1', tenantId: 1 }]
    };

    apiServiceSpy.getTenants.and.returnValue(of(tenantsResponse));
    apiServiceSpy.getCompanies.and.returnValue(of(companiesResponse));

    fixture = TestBed.createComponent(CompanyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants.length).toBe(1);
    expect(component.companies.length).toBe(1);
    expect(component.companies[0].companyName).toBe('Company 1');
  });
});
