import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { FormBuilder } from '@angular/forms';

import { TenantListComponent } from './tenant-list.component';
import { ApiService } from '../../core/services/api.service';
import { ApiResponse } from '../../core/models/phase01.models';

describe('TenantListComponent', () => {
  let component: TenantListComponent;
  let fixture: ComponentFixture<TenantListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', ['getTenants', 'createTenant', 'updateTenant', 'deleteTenant']);

    await TestBed.configureTestingModule({
      imports: [TenantListComponent],
      providers: [
        FormBuilder,
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should load tenants on init', () => {
    const tenantsResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [
        { tenantId: 1, tenantCode: 'T1', tenantName: 'Tenant 1', status: 'Active', effectiveFrom: '2026-01-01' }
      ]
    };
    apiServiceSpy.getTenants.and.returnValue(of(tenantsResponse));

    fixture = TestBed.createComponent(TenantListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants.length).toBe(1);
    expect(component.tenants[0].tenantName).toBe('Tenant 1');
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
