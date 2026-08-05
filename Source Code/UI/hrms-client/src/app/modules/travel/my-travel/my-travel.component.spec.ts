import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyTravelComponent } from './my-travel.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('MyTravelComponent', () => {
  let component: MyTravelComponent;
  let fixture: ComponentFixture<MyTravelComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getTravelRequests', 'getTravelAdvances', 'getExpenseClaims']);
    apiSpy.getTravelRequests.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getTravelAdvances.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getExpenseClaims.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [MyTravelComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(MyTravelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
