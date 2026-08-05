import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetInventoryComponent } from './asset-inventory.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetInventoryComponent', () => {
  let component: AssetInventoryComponent;
  let fixture: ComponentFixture<AssetInventoryComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getInventory']);
    apiSpy.getInventory.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetInventoryComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});