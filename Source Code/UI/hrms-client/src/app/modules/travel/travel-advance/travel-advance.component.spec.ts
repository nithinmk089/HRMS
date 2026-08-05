import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelAdvanceComponent } from './travel-advance.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelAdvanceComponent', () => {
  let component: TravelAdvanceComponent;
  let fixture: ComponentFixture<TravelAdvanceComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelAdvances', 'createTravelAdvance', 'approveTravelAdvance', 'disburseTravelAdvance']);
    apiSpy.getTravelAdvances.and.returnValue(of({ success: true, data: [] }));
    apiSpy.createTravelAdvance.and.returnValue(of({ success: true, data: 1 }));
    apiSpy.approveTravelAdvance.and.returnValue(of({ success: true, data: true }));
    apiSpy.disburseTravelAdvance.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [TravelAdvanceComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelAdvanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
