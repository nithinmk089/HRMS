import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetRegisterComponent } from './asset-register.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetRegisterComponent', () => {
  let component: AssetRegisterComponent;
  let fixture: ComponentFixture<AssetRegisterComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getAssets', 'getCategories', 'createAsset']);
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getCategories.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetRegisterComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});