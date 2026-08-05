import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of } from 'rxjs';
import { TransferListComponent } from './transfer-list.component';
import { ApiService } from '../../core/services/api.service';

describe('TransferListComponent', () => {
  let component: TransferListComponent;
  let fixture: ComponentFixture<TransferListComponent>;
  let apiServiceSpy: jasmine.SpyObj<ApiService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ApiService', [
      'getTenants',
      'getEmployees',
      'getTransfers',
      'createTransfer',
      'approveTransfer',
      'completeTransfer'
    ]);

    await TestBed.configureTestingModule({
      imports: [TransferListComponent],
      providers: [
        { provide: ApiService, useValue: spy }
      ]
    }).compileComponents();

    apiServiceSpy = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
  });

  it('should initialize and load tenants', () => {
    apiServiceSpy.getTenants.and.returnValue(of({ success: true, message: 'Loaded', data: [] }));

    fixture = TestBed.createComponent(TransferListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(component.tenants).toEqual([]);
    expect(apiServiceSpy.getTenants).toHaveBeenCalled();
  });
});
