import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SuccessionManagementComponent } from './succession-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('SuccessionManagementComponent', () => {
  let component: SuccessionManagementComponent;
  let fixture: ComponentFixture<SuccessionManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getSelfAssessments', 'getEmployees']);
    apiSpy.getSelfAssessments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [SuccessionManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SuccessionManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});