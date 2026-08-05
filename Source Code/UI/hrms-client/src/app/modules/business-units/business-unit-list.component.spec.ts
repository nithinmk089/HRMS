import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { BusinessUnitListComponent } from './business-unit-list.component';
import { ApiService } from '../../core/services/api.service';

describe('BusinessUnitListComponent', () => {
  let component: BusinessUnitListComponent;
  let fixture: ComponentFixture<BusinessUnitListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getBusinessUnits',
      'createBusinessUnit',
      'updateBusinessUnit',
      'deleteBusinessUnit'
    ]);

    await TestBed.configureTestingModule({
      imports: [BusinessUnitListComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(BusinessUnitListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
