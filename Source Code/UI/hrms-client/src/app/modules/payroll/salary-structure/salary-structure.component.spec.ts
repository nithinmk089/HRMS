import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SalaryStructureComponent } from './salary-structure.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('SalaryStructureComponent', () => {
  let component: SalaryStructureComponent;
  let fixture: ComponentFixture<SalaryStructureComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [SalaryStructureComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SalaryStructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});