import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelRequestComponent } from './travel-request.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelRequestComponent', () => {
  let component: TravelRequestComponent;
  let fixture: ComponentFixture<TravelRequestComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelRequests', 'createTravelRequest', 'cancelTravelRequest']);
    apiSpy.getTravelRequests.and.returnValue(of({ success: true, data: [] }));
    apiSpy.createTravelRequest.and.returnValue(of({ success: true, data: 1 }));
    apiSpy.cancelTravelRequest.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [TravelRequestComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
