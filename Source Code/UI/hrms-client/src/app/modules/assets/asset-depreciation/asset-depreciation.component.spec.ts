import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetDepreciationComponent } from './asset-depreciation.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetDepreciationComponent', () => {
  let component: AssetDepreciationComponent;
  let fixture: ComponentFixture<AssetDepreciationComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getDepreciationReport', 'recalculateDepreciation']);
    apiSpy.getDepreciationReport.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetDepreciationComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetDepreciationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});