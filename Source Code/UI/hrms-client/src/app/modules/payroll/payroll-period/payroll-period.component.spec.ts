import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PayrollPeriodComponent } from './payroll-period.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PayrollPeriodComponent', () => {
  let component: PayrollPeriodComponent;
  let fixture: ComponentFixture<PayrollPeriodComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PayrollPeriodComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PayrollPeriodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});