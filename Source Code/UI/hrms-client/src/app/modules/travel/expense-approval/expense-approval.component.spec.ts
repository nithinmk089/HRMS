import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpenseApprovalComponent } from './expense-approval.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('ExpenseApprovalComponent', () => {
  let component: ExpenseApprovalComponent;
  let fixture: ComponentFixture<ExpenseApprovalComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getExpenseClaims', 'approveExpenseClaim']);
    apiSpy.getExpenseClaims.and.returnValue(of({ success: true, data: [] }));
    apiSpy.approveExpenseClaim.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [ExpenseApprovalComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(ExpenseApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
