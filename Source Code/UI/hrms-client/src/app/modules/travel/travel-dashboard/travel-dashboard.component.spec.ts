import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelDashboardComponent } from './travel-dashboard.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelDashboardComponent', () => {
  let component: TravelDashboardComponent;
  let fixture: ComponentFixture<TravelDashboardComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelRequests', 'getExpenseClaims', 'getTravelAdvances', 'getTravelCompliance']);
    apiSpy.getTravelRequests.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getExpenseClaims.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getTravelAdvances.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getTravelCompliance.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [TravelDashboardComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
