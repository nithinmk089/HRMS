import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { NotificationTemplatesComponent } from './notification-templates.component';
import { NotificationService } from '../../core/services/notification.service';
import { ApiResponse } from '../../core/models/phase01.models';
import { NotificationTemplate } from '../../core/models/notification.models';

describe('NotificationTemplatesComponent', () => {
  let component: NotificationTemplatesComponent;
  let fixture: ComponentFixture<NotificationTemplatesComponent>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('NotificationService', ['getTemplates', 'createTemplate', 'updateTemplate', 'deleteTemplate']);

    await TestBed.configureTestingModule({
      imports: [NotificationTemplatesComponent],
      providers: [
        { provide: NotificationService, useValue: spy }
      ]
    }).compileComponents();

    notificationServiceSpy = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should load templates on init', () => {
    const mockTemplates: NotificationTemplate[] = [
      {
        templateId: 1,
        tenantId: 1,
        templateName: 'Welcome Template',
        businessEvent: 'Tenant Created',
        channel: 'Email',
        subjectTemplate: 'Welcome',
        bodyTemplate: 'Hello {AdminName}',
        isActive: true,
        versionNo: 1
      }
    ];

    notificationServiceSpy.getTemplates.and.returnValue(of({ success: true, message: 'Ok', data: mockTemplates }));

    fixture = TestBed.createComponent(NotificationTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    expect(component.templates.length).toBe(1);
    expect(component.templates[0].templateName).toBe('Welcome Template');
  });

  it('should open edit form when editTemplate is called', () => {
    notificationServiceSpy.getTemplates.and.returnValue(of({ success: true, message: 'Ok', data: [] }));

    fixture = TestBed.createComponent(NotificationTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    const template: NotificationTemplate = {
      templateId: 5,
      tenantId: 1,
      templateName: 'Template 5',
      businessEvent: 'Payment Failed',
      channel: 'SMS',
      bodyTemplate: 'Failed',
      isActive: true,
      versionNo: 2
    };

    component.editTemplate(template);

    expect(component.isEditing).toBeTrue();
    expect(component.selectedTemplate.templateId).toBe(5);
    expect(component.selectedTemplate.templateName).toBe('Template 5');
  });

  it('should create template when saveTemplate is called without ID', () => {
    notificationServiceSpy.getTemplates.and.returnValue(of({ success: true, message: 'Ok', data: [] }));
    notificationServiceSpy.createTemplate.and.returnValue(of({ success: true, message: 'Created', data: 10 }));

    fixture = TestBed.createComponent(NotificationTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    component.addNewTemplate();
    component.selectedTemplate.templateName = 'New Temp';
    component.selectedTemplate.bodyTemplate = 'Welcome text';

    component.saveTemplate();

    expect(notificationServiceSpy.createTemplate).toHaveBeenCalledWith(component.selectedTemplate);
    expect(component.isEditing).toBeFalse();
  });

  it('should update template when saveTemplate is called with ID', () => {
    notificationServiceSpy.getTemplates.and.returnValue(of({ success: true, message: 'Ok', data: [] }));
    notificationServiceSpy.updateTemplate.and.returnValue(of({ success: true, message: 'Updated', data: true }));

    fixture = TestBed.createComponent(NotificationTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    component.selectedTemplate = {
      templateId: 12,
      tenantId: 1,
      templateName: 'Updated Temp',
      bodyTemplate: 'Welcome update',
      businessEvent: 'Tenant Created',
      channel: 'Email'
    };

    component.saveTemplate();

    expect(notificationServiceSpy.updateTemplate).toHaveBeenCalledWith(12, component.selectedTemplate);
    expect(component.isEditing).toBeFalse();
  });

  it('should delete template on deleteTemplate confirmation', () => {
    notificationServiceSpy.getTemplates.and.returnValue(of({ success: true, message: 'Ok', data: [] }));
    notificationServiceSpy.deleteTemplate.and.returnValue(of({ success: true, message: 'Deleted', data: true }));

    fixture = TestBed.createComponent(NotificationTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    spyOn(window, 'confirm').and.returnValue(true);

    component.deleteTemplate(42);

    expect(notificationServiceSpy.deleteTemplate).toHaveBeenCalledWith(42, 1);
  });
});
