import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComplianceManagementComponent } from './compliance-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('ComplianceManagementComponent', () => {
  let component: ComplianceManagementComponent;
  let fixture: ComponentFixture<ComplianceManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [ComplianceManagementComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(ComplianceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});