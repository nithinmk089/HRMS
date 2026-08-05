import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { LeaveManagementComponent } from './leave-management.component';
import { ApiService } from '../../core/services/api.service';

describe('LeaveManagementComponent', () => {
  let component: LeaveManagementComponent;
  let fixture: ComponentFixture<LeaveManagementComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getLeaveTypes',
      'getLeaveRequests',
      'createLeaveType',
      'createLeavePolicy',
      'approveLeaveRequest',
      'rejectLeaveRequest'
    ]);

    await TestBed.configureTestingModule({
      imports: [LeaveManagementComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(LeaveManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
