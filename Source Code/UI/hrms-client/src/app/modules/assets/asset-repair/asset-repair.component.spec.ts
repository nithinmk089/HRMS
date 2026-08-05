import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetRepairComponent } from './asset-repair.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetRepairComponent', () => {
  let component: AssetRepairComponent;
  let fixture: ComponentFixture<AssetRepairComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getRepairs', 'getAssets', 'createRepair', 'closeRepair']);
    apiSpy.getRepairs.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetRepairComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetRepairComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});