import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { OvertimeManagementComponent } from './overtime-management.component';
import { ApiService } from '../../core/services/api.service';

describe('OvertimeManagementComponent', () => {
  let component: OvertimeManagementComponent;
  let fixture: ComponentFixture<OvertimeManagementComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getOvertimeRequests',
      'approveOvertimeRequest',
      'rejectOvertimeRequest'
    ]);

    await TestBed.configureTestingModule({
      imports: [OvertimeManagementComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(OvertimeManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
