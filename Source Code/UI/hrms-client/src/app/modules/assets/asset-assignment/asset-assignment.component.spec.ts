import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetAssignmentComponent } from './asset-assignment.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetAssignmentComponent', () => {
  let component: AssetAssignmentComponent;
  let fixture: ComponentFixture<AssetAssignmentComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getAssignments', 'getEmployees', 'getAssets', 'assignAsset', 'returnAsset']);
    apiSpy.getAssignments.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getAssets.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetAssignmentComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});