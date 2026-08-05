import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CorporateCardComponent } from './corporate-card.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('CorporateCardComponent', () => {
  let component: CorporateCardComponent;
  let fixture: ComponentFixture<CorporateCardComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCorporateCardTransactions', 'importCorporateCardTransaction', 'reconcileCorporateCardTransaction']);
    apiSpy.getCorporateCardTransactions.and.returnValue(of({ success: true, data: [] }));
    apiSpy.importCorporateCardTransaction.and.returnValue(of({ success: true, data: 1 }));
    apiSpy.reconcileCorporateCardTransaction.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [CorporateCardComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(CorporateCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
