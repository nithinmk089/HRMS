import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DevelopmentPlanComponent } from './development-plan.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('DevelopmentPlanComponent', () => {
  let component: DevelopmentPlanComponent;
  let fixture: ComponentFixture<DevelopmentPlanComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [DevelopmentPlanComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DevelopmentPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});