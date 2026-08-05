import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { NotificationPreferencesComponent } from './notification-preferences.component';
import { NotificationService } from '../../core/services/notification.service';
import { ApiResponse } from '../../core/models/phase01.models';
import { UserNotificationPreference } from '../../core/models/notification.models';

describe('NotificationPreferencesComponent', () => {
  let component: NotificationPreferencesComponent;
  let fixture: ComponentFixture<NotificationPreferencesComponent>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('NotificationService', ['getPreferences', 'savePreference']);

    await TestBed.configureTestingModule({
      imports: [NotificationPreferencesComponent],
      providers: [
        { provide: NotificationService, useValue: spy }
      ]
    }).compileComponents();

    notificationServiceSpy = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should load preferences and build matrix on init', () => {
    const mockPreferences: UserNotificationPreference[] = [
      {
        preferenceId: 1,
        tenantId: 1,
        userID: 1,
        businessEvent: 'Tenant Created',
        channel: 'Email',
        frequency: 'Daily Digest',
        isEnabled: true
      }
    ];

    notificationServiceSpy.getPreferences.and.returnValue(of({ success: true, message: 'Ok', data: mockPreferences }));

    fixture = TestBed.createComponent(NotificationPreferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    
    // Check if the preference was loaded into the matrix
    const pref = component.getPref('Tenant Created', 'Email');
    expect(pref).toBeTruthy();
    expect(pref.preferenceId).toBe(1);
    expect(pref.isEnabled).toBeTrue();
    expect(pref.frequency).toBe('Daily Digest');

    // Check default not set (should be initialized to isEnabled: false)
    const defaultPref = component.getPref('Payment Failed', 'SMS');
    expect(defaultPref).toBeTruthy();
    expect(defaultPref.preferenceId).toBe(0);
    expect(defaultPref.isEnabled).toBeFalse();
  });

  it('should call savePreference on toggle', () => {
    notificationServiceSpy.getPreferences.and.returnValue(of({ success: true, message: 'Ok', data: [] }));
    notificationServiceSpy.savePreference.and.returnValue(of({ success: true, message: 'Saved', data: true }));

    fixture = TestBed.createComponent(NotificationPreferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    const pref = component.getPref('Tenant Created', 'Email');
    pref.isEnabled = true;
    
    component.togglePreference('Tenant Created', 'Email');

    expect(notificationServiceSpy.savePreference).toHaveBeenCalledWith(pref);
  });

  it('should call savePreference on frequency change if enabled', () => {
    notificationServiceSpy.getPreferences.and.returnValue(of({ success: true, message: 'Ok', data: [] }));
    notificationServiceSpy.savePreference.and.returnValue(of({ success: true, message: 'Saved', data: true }));

    fixture = TestBed.createComponent(NotificationPreferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    const pref = component.getPref('Tenant Created', 'Email');
    pref.isEnabled = true;
    pref.frequency = 'Weekly Digest';

    component.changeFrequency('Tenant Created', 'Email');

    expect(notificationServiceSpy.savePreference).toHaveBeenCalledWith(pref);
  });
});
