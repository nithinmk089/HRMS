import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { AttendanceManagementComponent } from './attendance-management.component';
import { ApiService } from '../../core/services/api.service';

describe('AttendanceManagementComponent', () => {
  let component: AttendanceManagementComponent;
  let fixture: ComponentFixture<AttendanceManagementComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getEmployees',
      'getAttendanceRecords',
      'recalculate'
    ]);

    await TestBed.configureTestingModule({
      imports: [AttendanceManagementComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(AttendanceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
