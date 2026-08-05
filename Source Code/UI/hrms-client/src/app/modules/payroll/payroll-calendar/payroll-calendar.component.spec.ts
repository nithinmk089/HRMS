import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PayrollCalendarComponent } from './payroll-calendar.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PayrollCalendarComponent', () => {
  let component: PayrollCalendarComponent;
  let fixture: ComponentFixture<PayrollCalendarComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PayrollCalendarComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PayrollCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});