import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { SelfServiceProfileComponent } from './self-service-profile.component';
import { ApiService } from '../../core/services/api.service';

describe('SelfServiceProfileComponent', () => {
  let component: SelfServiceProfileComponent;
  let fixture: ComponentFixture<SelfServiceProfileComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getEmployee',
      'getEmploymentDetails',
      'getAddresses',
      'getContacts',
      'getEmergencyContacts',
      'getQualifications',
      'getCertifications',
      'getDocuments',
      'getServiceHistoryReport',
      'updateEmployeeProfile',
      'updateEmployeeContactDetails',
      'uploadEmployeeDocument',
      'getEmployeeServiceHistory'
    ]);

    await TestBed.configureTestingModule({
      imports: [SelfServiceProfileComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize profile data', () => {
    apiServiceSpy.getEmployee.and.returnValue(of({ success: true, message: 'Loaded', data: { employeeID: 1, firstName: 'Test', lastName: 'User' } as any }));
    apiServiceSpy.getEmploymentDetails.and.returnValue(of({ success: true, message: 'Loaded', data: {} as any }));
    apiServiceSpy.getAddresses.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getContacts.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getEmergencyContacts.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getQualifications.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getCertifications.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getDocuments.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getServiceHistoryReport.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(SelfServiceProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.employeeId).toBe(1);
    expect(apiServiceSpy.getEmployee).toHaveBeenCalledWith(1, 1);
  });
});
