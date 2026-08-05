import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CertificationManagementComponent } from './certification-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('CertificationManagementComponent', () => {
  let component: CertificationManagementComponent;
  let fixture: ComponentFixture<CertificationManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [CertificationManagementComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(CertificationManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});