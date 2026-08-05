import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { FormBuilder } from '@angular/forms';

import { UserListComponent } from './user-list.component';
import { ApiService } from '../../core/services/api.service';
import { ApiResponse } from '../../core/models/phase01.models';

describe('UserListComponent', () => {
  let component: UserListComponent;
  let fixture: ComponentFixture<UserListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', ['getTenants', 'getUsers', 'createUser', 'updateUser', 'deleteUser', 'lockUser', 'unlockUser']);

    await TestBed.configureTestingModule({
      imports: [UserListComponent],
      providers: [
        FormBuilder,
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should load tenants and users on init', () => {
    const tenantsResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [{ tenantId: 1, tenantCode: 'T1', tenantName: 'Tenant 1', status: 'Active' }]
    };
    const usersResponse: ApiResponse<any[]> = {
      success: true,
      message: 'Success',
      data: [{ userId: 1, userName: 'user1', email: 'user1@hrms.com', tenantId: 1, isLocked: false }]
    };

    apiServiceSpy.getTenants.and.returnValue(of(tenantsResponse));
    apiServiceSpy.getUsers.and.returnValue(of(usersResponse));

    fixture = TestBed.createComponent(UserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants.length).toBe(1);
    expect(component.users.length).toBe(1);
    expect(component.users[0].userName).toBe('user1');
  });
});
