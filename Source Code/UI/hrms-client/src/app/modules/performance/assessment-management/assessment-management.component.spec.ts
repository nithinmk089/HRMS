import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssessmentManagementComponent } from './assessment-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssessmentManagementComponent', () => {
  let component: AssessmentManagementComponent;
  let fixture: ComponentFixture<AssessmentManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssessmentManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssessmentManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});