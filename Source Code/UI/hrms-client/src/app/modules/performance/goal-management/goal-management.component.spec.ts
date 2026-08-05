import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GoalManagementComponent } from './goal-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('GoalManagementComponent', () => {
  let component: GoalManagementComponent;
  let fixture: ComponentFixture<GoalManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [GoalManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(GoalManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});