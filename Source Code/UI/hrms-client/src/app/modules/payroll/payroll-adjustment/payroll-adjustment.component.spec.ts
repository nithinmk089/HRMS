import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PayrollAdjustmentComponent } from './payroll-adjustment.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PayrollAdjustmentComponent', () => {
  let component: PayrollAdjustmentComponent;
  let fixture: ComponentFixture<PayrollAdjustmentComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PayrollAdjustmentComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PayrollAdjustmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});