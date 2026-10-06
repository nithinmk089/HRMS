import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-learning-progress',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './learning-progress.component.html',
  styleUrls: ['./learning-progress.component.scss']
})
export class LearningProgressComponent implements OnInit {
  items: any[] = [];
  employees: any[] = [];
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
    this.loadEmployees();
    this.loadCourses();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', Validators.required],
      courseId: ['', Validators.required]
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getLearningEnrollments(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load learning enrollments';
        this.isLoading = false;
      }
    });
  }

  loadEmployees(): void {
    this.api.getEmployees(this.tenantId).subscribe({
      next: (res) => {
        this.employees = res.data || [];
      },
      error: (err) => console.error('Failed to load employees', err)
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
      employeeId: this.employees.length > 0 ? this.employees[0].employeeId : '',
      courseId: this.courses.length > 0 ? this.courses[0].courseID : ''
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
      employeeId: Number(this.form.value.employeeId),
      courseId: Number(this.form.value.courseId)
    };

    this.api.createLearningEnrollment(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = res.message || 'Employee enrolled into course successfully!';
        this.closeModal();
        this.loadData();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Failed to enroll employee.';
      }
    });
  }
}