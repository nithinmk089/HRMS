import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelComplianceComponent } from './travel-compliance.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelComplianceComponent', () => {
  let component: TravelComplianceComponent;
  let fixture: ComponentFixture<TravelComplianceComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelCompliance']);
    apiSpy.getTravelCompliance.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [TravelComplianceComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelComplianceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
