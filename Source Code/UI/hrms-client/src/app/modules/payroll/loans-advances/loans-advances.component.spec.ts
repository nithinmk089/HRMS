import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoansAdvancesComponent } from './loans-advances.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('LoansAdvancesComponent', () => {
  let component: LoansAdvancesComponent;
  let fixture: ComponentFixture<LoansAdvancesComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getEmployeeCompensations', 'getEmployees']);
    apiSpy.getEmployeeCompensations.and.returnValue(of({ success: true, data: [] }));
    apiSpy.getEmployees.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [LoansAdvancesComponent],
      providers: [
        { provide: ApiService, useValue: apiSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoansAdvancesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});