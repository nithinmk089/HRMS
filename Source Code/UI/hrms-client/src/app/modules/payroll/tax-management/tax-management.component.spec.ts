import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaxManagementComponent } from './tax-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TaxManagementComponent', () => {
  let component: TaxManagementComponent;
  let fixture: ComponentFixture<TaxManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [TaxManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TaxManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});