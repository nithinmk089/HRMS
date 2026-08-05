import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { NotificationHistoryComponent } from './notification-history.component';
import { NotificationService } from '../../core/services/notification.service';
import { ApiResponse } from '../../core/models/phase01.models';
import { NotificationQueue } from '../../core/models/notification.models';

describe('NotificationHistoryComponent', () => {
  let component: NotificationHistoryComponent;
  let fixture: ComponentFixture<NotificationHistoryComponent>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('NotificationService', ['getHistory', 'retryNotification', 'cancelNotification', 'readNotification']);

    await TestBed.configureTestingModule({
      imports: [NotificationHistoryComponent],
      providers: [
        { provide: NotificationService, useValue: spy }
      ]
    }).compileComponents();

    notificationServiceSpy = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should load history on init', () => {
    const mockHistory: NotificationQueue[] = [
      {
        notificationQueueId: 101,
        tenantId: 1,
        recipient: 'test@example.com',
        channel: 'Email',
        businessEvent: 'Tenant Created',
        subject: 'Welcome',
        body: 'Hello',
        status: 'Delivered',
        retryCount: 0,
        maxRetries: 3,
        nextRunDate: '2026-01-01',
        createdDate: '2026-01-01',
        escalationStatus: 'None'
      }
    ];

    const mockResponse: ApiResponse<NotificationQueue[]> = {
      success: true,
      message: 'Success',
      data: mockHistory
    };

    notificationServiceSpy.getHistory.and.returnValue(of(mockResponse));

    fixture = TestBed.createComponent(NotificationHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    expect(component.history.length).toBe(1);
    expect(component.history[0].recipient).toBe('test@example.com');
  });

  it('should call cancel and reload history', () => {
    const mockHistory: NotificationQueue[] = [];
    notificationServiceSpy.getHistory.and.returnValue(of({ success: true, message: 'Ok', data: mockHistory }));
    notificationServiceSpy.cancelNotification.and.returnValue(of({ success: true, message: 'Cancelled', data: true }));

    fixture = TestBed.createComponent(NotificationHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    spyOn(window, 'confirm').and.returnValue(true);

    component.cancel(101);

    expect(notificationServiceSpy.cancelNotification).toHaveBeenCalledWith(101, 1);
    expect(notificationServiceSpy.getHistory).toHaveBeenCalled();
  });

  it('should call retry and reload history', () => {
    const mockHistory: NotificationQueue[] = [];
    notificationServiceSpy.getHistory.and.returnValue(of({ success: true, message: 'Ok', data: mockHistory }));
    notificationServiceSpy.retryNotification.and.returnValue(of({ success: true, message: 'Queued', data: true }));

    fixture = TestBed.createComponent(NotificationHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    component.retry(101);

    expect(notificationServiceSpy.retryNotification).toHaveBeenCalledWith(101, 1);
    expect(notificationServiceSpy.getHistory).toHaveBeenCalled();
  });

  it('should call markAsRead and reload history', () => {
    const mockHistory: NotificationQueue[] = [];
    notificationServiceSpy.getHistory.and.returnValue(of({ success: true, message: 'Ok', data: mockHistory }));
    notificationServiceSpy.readNotification.and.returnValue(of({ success: true, message: 'Read', data: true }));

    fixture = TestBed.createComponent(NotificationHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    component.markAsRead(101);

    expect(notificationServiceSpy.readNotification).toHaveBeenCalledWith(101, 1);
    expect(notificationServiceSpy.getHistory).toHaveBeenCalled();
  });
});
