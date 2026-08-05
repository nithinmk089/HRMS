import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { OnboardingWorkflowComponent } from './onboarding-workflow.component';
import { ApiService } from '../../core/services/api.service';

describe('OnboardingWorkflowComponent', () => {
  let component: OnboardingWorkflowComponent;
  let fixture: ComponentFixture<OnboardingWorkflowComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getOnboardingStatusReport',
      'getPendingOnboardingTasksReport',
      'createOnboardingWorkflow',
      'startOnboardingWorkflow',
      'completeOnboardingWorkflow',
      'completeOnboardingTaskAssignment'
    ]);

    await TestBed.configureTestingModule({
      imports: [OnboardingWorkflowComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should load reports on init', () => {
    apiServiceSpy.getOnboardingStatusReport.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));
    apiServiceSpy.getPendingOnboardingTasksReport.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(OnboardingWorkflowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.statusReports).toEqual([]);
    expect(component.pendingTasks).toEqual([]);
    expect(apiServiceSpy.getOnboardingStatusReport).toHaveBeenCalled();
  });
});
