import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetAuditComponent } from './asset-audit.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetAuditComponent', () => {
  let component: AssetAuditComponent;
  let fixture: ComponentFixture<AssetAuditComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getAudits', 'getAssets', 'getEmployees', 'createAudit', 'completeAudit']);
    apiSpy.getAudits.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetAuditComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});