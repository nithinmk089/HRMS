import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpenseSettlementComponent } from './expense-settlement.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('ExpenseSettlementComponent', () => {
  let component: ExpenseSettlementComponent;
  let fixture: ComponentFixture<ExpenseSettlementComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getExpenseClaims', 'processExpenseSettlement']);
    apiSpy.getExpenseClaims.and.returnValue(of({ success: true, data: [] }));
    apiSpy.processExpenseSettlement.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [ExpenseSettlementComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(ExpenseSettlementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
