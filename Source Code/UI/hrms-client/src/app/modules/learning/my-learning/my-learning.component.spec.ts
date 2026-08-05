import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyLearningComponent } from './my-learning.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('MyLearningComponent', () => {
  let component: MyLearningComponent;
  let fixture: ComponentFixture<MyLearningComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [MyLearningComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(MyLearningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});