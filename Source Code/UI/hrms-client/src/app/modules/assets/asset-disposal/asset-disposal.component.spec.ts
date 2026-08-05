import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetDisposalComponent } from './asset-disposal.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetDisposalComponent', () => {
  let component: AssetDisposalComponent;
  let fixture: ComponentFixture<AssetDisposalComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getDisposals', 'getAssets', 'createDisposal', 'approveDisposal', 'closeDisposal']);
    apiSpy.getDisposals.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetDisposalComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetDisposalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});