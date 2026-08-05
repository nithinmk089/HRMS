import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LearningDashboardComponent } from './learning-dashboard.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('LearningDashboardComponent', () => {
  let component: LearningDashboardComponent;
  let fixture: ComponentFixture<LearningDashboardComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [LearningDashboardComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(LearningDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});