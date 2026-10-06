import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-assessment-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './assessment-management.component.html',
  styleUrls: ['./assessment-management.component.scss']
})
export class AssessmentManagementComponent implements OnInit {
  items: any[] = [];
  courses: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';
  isLoading = false;
  tenantId = 1;

  constructor(
    private api: ApiService,
    private auth: AuthService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.tenantId = this.auth.getTenantId() || 1;
    this.initForm();
    this.loadData();
    this.loadCourses();
  }

  initForm(): void {
    this.form = this.fb.group({
      courseId: ['', Validators.required],
      assessmentName: ['', [Validators.required, Validators.maxLength(200)]],
      passPercentage: [70, [Validators.required, Validators.min(1), Validators.max(100)]]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getLearningAssessments(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load assessments';
        this.isLoading = false;
      }
    });
  }

  loadCourses(): void {
    this.api.getCourses(this.tenantId).subscribe({
      next: (res) => {
        this.courses = res.data || [];
      },
      error: (err) => console.error('Failed to load courses', err)
    });
  }

  openCreateModal(): void {
    this.form.reset({
      courseId: this.courses.length > 0 ? this.courses[0].courseID : '',
      assessmentName: '',
      passPercentage: 70
    });
    this.successMessage = '';
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';

    const payload = {
      tenantId: this.tenantId,
      courseId: Number(this.form.value.courseId),
      assessmentName: this.form.value.assessmentName,
      passPercentage: Number(this.form.value.passPercentage)
    };

    this.api.createLearningAssessment(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Course assessment created successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to create assessment.';
      }
    });
  }
}