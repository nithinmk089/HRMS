import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PerformanceCycleComponent } from './performance-cycle.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PerformanceCycleComponent', () => {
  let component: PerformanceCycleComponent;
  let fixture: ComponentFixture<PerformanceCycleComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PerformanceCycleComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PerformanceCycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});