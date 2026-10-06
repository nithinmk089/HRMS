import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-performance-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './performance-dashboard.component.html',
  styleUrls: ['./performance-dashboard.component.scss']
})
export class PerformanceDashboardComponent implements OnInit {
  cycles: any[] = [];
  goals: any[] = [];
  feedback: any[] = [];
  checkins: any[] = [];
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
    this.api.getPerformanceCycles(this.tenantId).subscribe({
      next: (res) => this.cycles = res.data || []
    });

    this.api.getGoals(this.tenantId).subscribe({
      next: (res) => {
        this.goals = res.data || [];
        this.isLoading = false;
      },
      error: () => this.isLoading = false
    });

    this.api.getContinuousFeedback(this.tenantId).subscribe({
      next: (res) => this.feedback = res.data || []
    });

    this.api.getCheckIns(this.tenantId).subscribe({
      next: (res) => this.checkins = res.data || []
    });
  }

  get completedGoalsCount(): number {
    return this.goals.filter(g => g.goalStatus === 'Completed' || (g.achievementPercentage && g.achievementPercentage >= 100)).length;
  }

  get avgGoalAchievement(): number {
    if (this.goals.length === 0) return 0;
    const total = this.goals.reduce((acc, g) => acc + (Number(g.achievementPercentage) || 0), 0);
    return Math.round(total / this.goals.length);
  }
}