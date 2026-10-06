import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-my-learning',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-learning.component.html',
  styleUrls: ['./my-learning.component.scss']
})
export class MyLearningComponent implements OnInit {
  assignments: any[] = [];
  courses: any[] = [];
  errorMessage = '';
  isLoading = false;
  tenantId = 1;
  employeeId: number | null = null;

  constructor(
    private api: ApiService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.tenantId = this.auth.getTenantId() || 1;
    this.employeeId = this.auth.getEmployeeId();
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getLearningAssignments(this.tenantId, this.employeeId ?? undefined).subscribe({
      next: (res) => {
        this.assignments = res.data || [];
        this.isLoading = false;
      },
      error: () => this.isLoading = false
    });

    this.api.getCourses(this.tenantId).subscribe({
      next: (res) => {
        this.courses = res.data || [];
      }
    });
  }
}