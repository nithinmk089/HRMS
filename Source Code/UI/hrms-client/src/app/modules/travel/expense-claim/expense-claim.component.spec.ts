import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpenseClaimComponent } from './expense-claim.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('ExpenseClaimComponent', () => {
  let component: ExpenseClaimComponent;
  let fixture: ComponentFixture<ExpenseClaimComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getExpenseClaims', 'getExpenseCategories', 'createExpenseClaim', 'submitExpenseClaim']);
    apiSpy.getExpenseClaims.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getExpenseCategories.and.returnValue(of({ success: true, data: [] }));
    apiSpy.createExpenseClaim.and.returnValue(of({ success: true, data: 1 }));
    apiSpy.submitExpenseClaim.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [ExpenseClaimComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(ExpenseClaimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
