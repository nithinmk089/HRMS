import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BonusManagementComponent } from './bonus-management.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('BonusManagementComponent', () => {
  let component: BonusManagementComponent;
  let fixture: ComponentFixture<BonusManagementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [BonusManagementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BonusManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});