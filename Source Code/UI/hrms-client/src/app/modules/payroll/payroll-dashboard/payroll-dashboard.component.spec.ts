import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PayrollDashboardComponent } from './payroll-dashboard.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PayrollDashboardComponent', () => {
  let component: PayrollDashboardComponent;
  let fixture: ComponentFixture<PayrollDashboardComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PayrollDashboardComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PayrollDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});