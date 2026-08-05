import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetWarrantyComponent } from './asset-warranty.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetWarrantyComponent', () => {
  let component: AssetWarrantyComponent;
  let fixture: ComponentFixture<AssetWarrantyComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getWarranties', 'getAssets', 'createWarranty']);
    apiSpy.getWarranties.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetWarrantyComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetWarrantyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});