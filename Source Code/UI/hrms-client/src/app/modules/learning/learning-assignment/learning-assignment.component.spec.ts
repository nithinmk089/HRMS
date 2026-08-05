import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LearningAssignmentComponent } from './learning-assignment.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('LearningAssignmentComponent', () => {
  let component: LearningAssignmentComponent;
  let fixture: ComponentFixture<LearningAssignmentComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [LearningAssignmentComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(LearningAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});