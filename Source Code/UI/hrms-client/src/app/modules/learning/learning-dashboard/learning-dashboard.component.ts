import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-learning-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './learning-dashboard.component.html',
  styleUrls: ['./learning-dashboard.component.scss']
})
export class LearningDashboardComponent implements OnInit {
  courses: any[] = [];
  categories: any[] = [];
  enrollments: any[] = [];
  assignments: any[] = [];
  isLoading = false;
  tenantId = 1;

  constructor(
    private api: ApiService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.tenantId = this.auth.getTenantId() || 1;
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.isLoading = true;
    this.api.getCourses(this.tenantId).subscribe({
      next: (res) => this.courses = res.data || []
    });

    this.api.getCourseCategories(this.tenantId).subscribe({
      next: (res) => this.categories = res.data || []
    });

    this.api.getLearningEnrollments(this.tenantId).subscribe({
      next: (res) => this.enrollments = res.data || []
    });

    this.api.getLearningAssignments(this.tenantId).subscribe({
      next: (res) => {
        this.assignments = res.data || [];
        this.isLoading = false;
      },
      error: () => this.isLoading = false
    });
  }

  get completedEnrollmentsCount(): number {
    return this.enrollments.filter(e => e.enrollmentStatus === 'Completed' || (e.completionPercentage && e.completionPercentage >= 100)).length;
  }
}