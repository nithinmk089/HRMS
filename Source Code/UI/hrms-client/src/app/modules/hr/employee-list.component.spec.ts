import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';
import { EmployeeListComponent } from './employee-list.component';
import { ApiService } from '../../core/services/api.service';

describe('EmployeeListComponent', () => {
  let component: EmployeeListComponent;
  let fixture: ComponentFixture<EmployeeListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getCompanies',
      'getBusinessUnits',
      'getDepartments',
      'getDesignations',
      'getLocations',
      'getCostCenters',
      'getEmployees',
      'createEmployee',
      'updateEmployee'
    ]);

    await TestBed.configureTestingModule({
      imports: [EmployeeListComponent],
      providers: [
        provideRouter([]),
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(EmployeeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
