import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SkillDevelopmentComponent } from './skill-development.component';
import { ApiService } from '../../../core/services/api.service';
import { of } from 'rxjs';

describe('SkillDevelopmentComponent', () => {
  let component: SkillDevelopmentComponent;
  let fixture: ComponentFixture<SkillDevelopmentComponent>;
  let apiSpy: any;

  beforeEach(async () => {
    apiSpy = jasmine.createSpyObj('ApiService', ['getCourses']);
    apiSpy.getCourses.and.returnValue(of({ success: true, data: [] }));

    await TestBed.configureTestingModule({
      imports: [SkillDevelopmentComponent],
      providers: [{ provide: ApiService, useValue: apiSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(SkillDevelopmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});