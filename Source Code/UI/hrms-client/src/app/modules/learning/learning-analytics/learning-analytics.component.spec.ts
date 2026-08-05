import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LearningAnalyticsComponent } from './learning-analytics.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('LearningAnalyticsComponent', () => {
  let component: LearningAnalyticsComponent;
  let fixture: ComponentFixture<LearningAnalyticsComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [LearningAnalyticsComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(LearningAnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});