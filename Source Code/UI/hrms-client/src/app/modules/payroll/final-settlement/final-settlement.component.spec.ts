import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FinalSettlementComponent } from './final-settlement.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('FinalSettlementComponent', () => {
  let component: FinalSettlementComponent;
  let fixture: ComponentFixture<FinalSettlementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [FinalSettlementComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FinalSettlementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});