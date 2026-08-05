import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CalibrationManagementComponent } from './calibration-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('CalibrationManagementComponent', () => {
  let component: CalibrationManagementComponent;
  let fixture: ComponentFixture<CalibrationManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [CalibrationManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CalibrationManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});