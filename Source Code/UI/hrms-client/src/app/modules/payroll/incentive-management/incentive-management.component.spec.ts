import { ComponentFixture, TestBed } from '@angular/core/testing';
import { IncentiveManagementComponent } from './incentive-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('IncentiveManagementComponent', () => {
  let component: IncentiveManagementComponent;
  let fixture: ComponentFixture<IncentiveManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [IncentiveManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(IncentiveManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});