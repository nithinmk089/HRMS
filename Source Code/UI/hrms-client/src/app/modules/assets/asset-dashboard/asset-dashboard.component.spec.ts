import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetDashboardComponent } from './asset-dashboard.component';
import { provideRouter } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetDashboardComponent', () => {
  let component: AssetDashboardComponent;
  let fixture: ComponentFixture<AssetDashboardComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getAssets', 'getWarrantyExpiryReport']);
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getWarrantyExpiryReport.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetDashboardComponent],
      providers: [
        provideRouter([]),
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});