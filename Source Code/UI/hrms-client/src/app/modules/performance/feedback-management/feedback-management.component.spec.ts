import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FeedbackManagementComponent } from './feedback-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('FeedbackManagementComponent', () => {
  let component: FeedbackManagementComponent;
  let fixture: ComponentFixture<FeedbackManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [FeedbackManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FeedbackManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});