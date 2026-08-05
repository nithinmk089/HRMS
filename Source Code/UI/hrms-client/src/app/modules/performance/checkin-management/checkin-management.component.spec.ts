import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CheckinManagementComponent } from './checkin-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('CheckinManagementComponent', () => {
  let component: CheckinManagementComponent;
  let fixture: ComponentFixture<CheckinManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [CheckinManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CheckinManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});