import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArrearsComponent } from './arrears.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('ArrearsComponent', () => {
  let component: ArrearsComponent;
  let fixture: ComponentFixture<ArrearsComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [ArrearsComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ArrearsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});