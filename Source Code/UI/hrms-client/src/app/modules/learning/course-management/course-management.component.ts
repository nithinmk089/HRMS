import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-course-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './course-management.component.html',
  styleUrls: ['./course-management.component.scss']
})
export class CourseManagementComponent implements OnInit {
  items: any[] = [];
  categories: any[] = [];
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
    this.loadCategories();
  }

  initForm(): void {
    this.form = this.fb.group({
      courseCode: ['', [Validators.required, Validators.maxLength(50)]],
      courseName: ['', [Validators.required, Validators.maxLength(200)]],
      courseCategoryId: ['', Validators.required],
      durationMinutes: [60, [Validators.required, Validators.min(5)]],
      description: ['']
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getCourses(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load courses';
        this.isLoading = false;
      }
    });
  }

  loadCategories(): void {
    this.api.getCourseCategories(this.tenantId).subscribe({
      next: (res) => {
        this.categories = res.data || [];
      },
      error: (err) => console.error('Failed to load course categories', err)
    });
  }

  openCreateModal(): void {
    const codeNum = Math.floor(Math.random() * 900 + 100);
    this.form.reset({
      courseCode: `CRS-${codeNum}`,
      courseName: '',
      courseCategoryId: this.categories.length > 0 ? this.categories[0].courseCategoryID : '',
      durationMinutes: 60,
      description: ''
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
      courseCode: this.form.value.courseCode,
      courseName: this.form.value.courseName,
      courseCategoryID: Number(this.form.value.courseCategoryId),
      durationMinutes: Number(this.form.value.durationMinutes),
      description: this.form.value.description
    };

    this.api.createCourse(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Course created successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to create course.';
      }
    });
  }
}