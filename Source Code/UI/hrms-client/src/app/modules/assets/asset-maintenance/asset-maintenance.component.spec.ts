import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetMaintenanceComponent } from './asset-maintenance.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetMaintenanceComponent', () => {
  let component: AssetMaintenanceComponent;
  let fixture: ComponentFixture<AssetMaintenanceComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getMaintenance', 'getAssets', 'createMaintenance', 'closeMaintenance']);
    apiSpy.getMaintenance.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetMaintenanceComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetMaintenanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});