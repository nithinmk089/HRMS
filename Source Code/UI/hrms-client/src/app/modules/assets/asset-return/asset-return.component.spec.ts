import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetReturnComponent } from './asset-return.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('AssetReturnComponent', () => {
  let component: AssetReturnComponent;
  let fixture: ComponentFixture<AssetReturnComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getReturns', 'verifyReturn']);
    apiSpy.getReturns.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [AssetReturnComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AssetReturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});