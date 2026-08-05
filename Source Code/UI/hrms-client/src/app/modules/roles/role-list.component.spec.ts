import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { FormBuilder } from '@angular/forms';

import { RoleListComponent } from './role-list.component';
import { ApiService } from '../../core/services/api.service';
import { ApiResponse } from '../../core/models/phase01.models';

describe('RoleListComponent', () => {
  let component: RoleListComponent;
  let fixture: ComponentFixture<RoleListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', ['getTenants', 'getRoles', 'createRole', 'updateRole', 'deleteRole']);

    await TestBed.configureTestingModule({
      imports: [RoleListComponent],
      providers: [
        FormBuilder,
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should load tenants and roles on init', () => {
    const tenantsResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [{ tenantId: 1, tenantCode: 'T1', tenantName: 'Tenant 1', status: 'Active' }]
    };
    const rolesResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [{ roleId: 1, roleCode: 'ADMIN', roleName: 'Administrator', tenantId: 1 }]
    };

    apiServiceSpy.getTenants.and.returnValue(of(tenantsResponse));
    apiServiceSpy.getRoles.and.returnValue(of(rolesResponse));

    fixture = TestBed.createComponent(RoleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants.length).toBe(1);
    expect(component.roles.length).toBe(1);
    expect(component.roles[0].roleName).toBe('Administrator');
  });
});
