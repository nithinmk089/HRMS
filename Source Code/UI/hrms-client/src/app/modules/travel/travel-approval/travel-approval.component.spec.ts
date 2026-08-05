import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TravelApprovalComponent } from './travel-approval.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('TravelApprovalComponent', () => {
  let component: TravelApprovalComponent;
  let fixture: ComponentFixture<TravelApprovalComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelRequests', 'approveTravelRequest']);
    apiSpy.getTravelRequests.and.returnValue(of({ success: true, data: [] }));
    apiSpy.approveTravelRequest.and.returnValue(of({ success: true, data: true }));

    await TestBed.configureTestingModule({
      imports: [TravelApprovalComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
