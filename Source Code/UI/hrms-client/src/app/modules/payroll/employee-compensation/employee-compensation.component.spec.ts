import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EmployeeCompensationComponent } from './employee-compensation.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('EmployeeCompensationComponent', () => {
  let component: EmployeeCompensationComponent;
  let fixture: ComponentFixture<EmployeeCompensationComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [EmployeeCompensationComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EmployeeCompensationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});