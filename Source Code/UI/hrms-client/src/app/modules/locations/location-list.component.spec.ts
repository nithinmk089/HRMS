import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { LocationListComponent } from './location-list.component';
import { ApiService } from '../../core/services/api.service';

describe('LocationListComponent', () => {
  let component: LocationListComponent;
  let fixture: ComponentFixture<LocationListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getLocations',
      'createLocation',
      'updateLocation',
      'deleteLocation'
    ]);

    await TestBed.configureTestingModule({
      imports: [LocationListComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(LocationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
