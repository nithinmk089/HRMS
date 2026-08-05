import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelPolicyComponent } from './travel-policy.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelPolicyComponent', () => {
  let component: TravelPolicyComponent;
  let fixture: ComponentFixture<TravelPolicyComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelPolicies', 'createTravelPolicy']);
    apiSpy.getTravelPolicies.and.returnValue(of({ success: true, data: [] }));
    apiSpy.createTravelPolicy.and.returnValue(of({ success: true, data: 1 }));

    await TestBed.configureTestingModule({
      imports: [TravelPolicyComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
