import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { OffboardingWorkflowComponent } from './offboarding-workflow.component';
import { ApiService } from '../../core/services/api.service';

describe('OffboardingWorkflowComponent', () => {
  let component: OffboardingWorkflowComponent;
  let fixture: ComponentFixture<OffboardingWorkflowComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getExitStatusReport',
      'getClearanceStatusReport',
      'getFullAndFinalSummaryReport',
      'submitExitRequest',
      'approveExitRequest',
      'rejectExitRequest',
      'initiateClearance',
      'calculateFullAndFinal'
    ]);

    await TestBed.configureTestingModule({
      imports: [OffboardingWorkflowComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should load offboarding data on init', () => {
    apiServiceSpy.getExitStatusReport.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getClearanceStatusReport.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getFullAndFinalSummaryReport.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(OffboardingWorkflowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.exitRequests).toEqual([]);
    expect(component.clearanceRequests).toEqual([]);
    expect(component.settlements).toEqual([]);
    expect(apiServiceSpy.getExitStatusReport).toHaveBeenCalled();
  });
});
