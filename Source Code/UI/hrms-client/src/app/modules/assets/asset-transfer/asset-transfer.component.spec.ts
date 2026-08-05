import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetTransferComponent } from './asset-transfer.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetTransferComponent', () => {
  let component: AssetTransferComponent;
  let fixture: ComponentFixture<AssetTransferComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTransfers', 'getEmployees', 'getAssets', 'getAssignments', 'createAssetTransfer', 'approveAssetTransfer', 'completeAssetTransfer']);
    apiSpy.getTransfers.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetTransferComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});