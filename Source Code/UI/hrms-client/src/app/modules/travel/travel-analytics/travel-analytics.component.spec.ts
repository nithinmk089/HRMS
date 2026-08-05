import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelAnalyticsComponent } from './travel-analytics.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelAnalyticsComponent', () => {
  let component: TravelAnalyticsComponent;
  let fixture: ComponentFixture<TravelAnalyticsComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelAnalyticsSummary']);
    apiSpy.getTravelAnalyticsSummary.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [TravelAnalyticsComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelAnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
