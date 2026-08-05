import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';
import { NotificationDashboardComponent } from './notification-dashboard.component';
import { NotificationService } from '../../core/services/notification.service';
import { ApiResponse } from '../../core/models/phase01.models';
import { NotificationMetrics } from '../../core/models/notification.models';

describe('NotificationDashboardComponent', () => {
  let component: NotificationDashboardComponent;
  let fixture: ComponentFixture<NotificationDashboardComponent>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('NotificationService', ['getMetrics']);

    await TestBed.configureTestingModule({
      imports: [NotificationDashboardComponent],
      providers: [
        provideRouter([]),
        { provide: NotificationService, useValue: spy }
      ]
    }).compileComponents();

    notificationServiceSpy = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should load metrics on init', () => {
    const mockMetrics: NotificationMetrics = {
      totalProcessed: 100,
      sentCount: 90,
      deliveredCount: 85,
      readCount: 50,
      failedCount: 10,
      deliveryRate: 85.0,
      failureRate: 10.0,
      readRate: 58.8
    };

    const mockResponse: ApiResponse<NotificationMetrics> = {
      success: true,
      message: 'Metrics retrieved',
      data: mockMetrics
    };

    notificationServiceSpy.getMetrics.and.returnValue(of(mockResponse));

    fixture = TestBed.createComponent(NotificationDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    expect(component.metrics).toEqual(mockMetrics);
    expect(notificationServiceSpy.getMetrics).toHaveBeenCalledWith(1);
  });
});
