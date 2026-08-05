import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PayrollRunComponent } from './payroll-run.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('PayrollRunComponent', () => {
  let component: PayrollRunComponent;
  let fixture: ComponentFixture<PayrollRunComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [PayrollRunComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PayrollRunComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});