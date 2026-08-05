import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PayslipManagementComponent } from './payslip-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PayslipManagementComponent', () => {
  let component: PayslipManagementComponent;
  let fixture: ComponentFixture<PayslipManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PayslipManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PayslipManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});