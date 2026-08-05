import { Routes } from '@angular/router';
import { LoginComponent } from './modules/auth/login/login.component';
import { ForgotPasswordComponent } from './modules/auth/forgot-password/forgot-password.component';
import { DashboardComponent } from './layout/dashboard/dashboard.component';
import { DashboardOverviewComponent } from './modules/dashboard/dashboard-overview.component';
import { TenantListComponent } from './modules/tenants/tenant-list.component';
import { CompanyListComponent } from './modules/companies/company-list.component';
import { BusinessUnitListComponent } from './modules/business-units/business-unit-list.component';
import { DepartmentListComponent } from './modules/departments/department-list.component';
import { DesignationListComponent } from './modules/designations/designation-list.component';
import { LocationListComponent } from './modules/locations/location-list.component';
import { CostCenterListComponent } from './modules/cost-centers/cost-center-list.component';
import { UserListComponent } from './modules/users/user-list.component';
import { RoleListComponent } from './modules/roles/role-list.component';
import { PermissionListComponent } from './modules/permissions/permission-list.component';
import { ConfigurationListComponent } from './modules/configurations/configuration-list.component';
import { NotificationHistoryComponent } from './modules/notifications/notification-history.component';
import { NotificationTemplatesComponent } from './modules/notifications/notification-templates.component';
import { NotificationPreferencesComponent } from './modules/notifications/notification-preferences.component';
import { NotificationDashboardComponent } from './modules/notifications/notification-dashboard.component';
import { EmployeeListComponent } from './modules/hr/employee-list.component';
import { EmployeeDetailComponent } from './modules/hr/employee-detail.component';
import { TransferListComponent } from './modules/hr/transfer-list.component';
import { PromotionListComponent } from './modules/hr/promotion-list.component';
import { SelfServiceProfileComponent } from './modules/hr/self-service-profile.component';
import { ShiftManagementComponent } from './modules/time-management/shift-management.component';
import { AttendanceManagementComponent } from './modules/time-management/attendance-management.component';
import { LeaveManagementComponent } from './modules/time-management/leave-management.component';
import { OvertimeManagementComponent } from './modules/time-management/overtime-management.component';
import { SelfServiceTimeComponent } from './modules/time-management/self-service-time.component';
import { OnboardingWorkflowComponent } from './modules/onboarding-offboarding/onboarding-workflow.component';
import { OffboardingWorkflowComponent } from './modules/onboarding-offboarding/offboarding-workflow.component';
import { AssetDashboardComponent } from './modules/assets/asset-dashboard/asset-dashboard.component';
import { AssetCategoryComponent } from './modules/assets/asset-category/asset-category.component';
import { AssetRegisterComponent } from './modules/assets/asset-register/asset-register.component';
import { AssetAssignmentComponent } from './modules/assets/asset-assignment/asset-assignment.component';
import { AssetTransferComponent } from './modules/assets/asset-transfer/asset-transfer.component';
import { AssetMaintenanceComponent } from './modules/assets/asset-maintenance/asset-maintenance.component';
import { AssetRepairComponent } from './modules/assets/asset-repair/asset-repair.component';
import { AssetWarrantyComponent } from './modules/assets/asset-warranty/asset-warranty.component';
import { AssetDepreciationComponent } from './modules/assets/asset-depreciation/asset-depreciation.component';
import { AssetAuditComponent } from './modules/assets/asset-audit/asset-audit.component';
import { AssetDisposalComponent } from './modules/assets/asset-disposal/asset-disposal.component';
import { AssetReturnComponent } from './modules/assets/asset-return/asset-return.component';
import { AssetInventoryComponent } from './modules/assets/asset-inventory/asset-inventory.component';

import { PayrollDashboardComponent } from './modules/payroll/payroll-dashboard/payroll-dashboard.component';
import { PayrollCalendarComponent } from './modules/payroll/payroll-calendar/payroll-calendar.component';
import { PayrollPeriodComponent } from './modules/payroll/payroll-period/payroll-period.component';
import { SalaryStructureComponent } from './modules/payroll/salary-structure/salary-structure.component';
import { EmployeeCompensationComponent } from './modules/payroll/employee-compensation/employee-compensation.component';
import { PayrollRunComponent } from './modules/payroll/payroll-run/payroll-run.component';
import { PayrollAdjustmentComponent } from './modules/payroll/payroll-adjustment/payroll-adjustment.component';
import { ArrearsComponent } from './modules/payroll/arrears/arrears.component';
import { LoansAdvancesComponent } from './modules/payroll/loans-advances/loans-advances.component';
import { BonusManagementComponent } from './modules/payroll/bonus-management/bonus-management.component';
import { IncentiveManagementComponent } from './modules/payroll/incentive-management/incentive-management.component';
import { TaxManagementComponent } from './modules/payroll/tax-management/tax-management.component';
import { PayslipManagementComponent } from './modules/payroll/payslip-management/payslip-management.component';
import { BankTransferComponent } from './modules/payroll/bank-transfer/bank-transfer.component';
import { FinalSettlementComponent } from './modules/payroll/final-settlement/final-settlement.component';

import { PerformanceDashboardComponent } from './modules/performance/performance-dashboard/performance-dashboard.component';
import { PerformanceCycleComponent } from './modules/performance/performance-cycle/performance-cycle.component';
import { GoalManagementComponent } from './modules/performance/goal-management/goal-management.component';
import { CompetencyFrameworkComponent } from './modules/performance/competency-framework/competency-framework.component';
import { FeedbackManagementComponent } from './modules/performance/feedback-management/feedback-management.component';
import { CheckinManagementComponent } from './modules/performance/checkin-management/checkin-management.component';
import { AssessmentManagementComponent } from './modules/performance/assessment-management/assessment-management.component';
import { CalibrationManagementComponent } from './modules/performance/calibration-management/calibration-management.component';
import { RatingManagementComponent } from './modules/performance/rating-management/rating-management.component';
import { DevelopmentPlanComponent } from './modules/performance/development-plan/development-plan.component';
import { PromotionManagementComponent } from './modules/performance/promotion-management/promotion-management.component';
import { SuccessionManagementComponent } from './modules/performance/succession-management/succession-management.component';

import { LearningDashboardComponent } from './modules/learning/learning-dashboard/learning-dashboard.component';
import { CourseManagementComponent } from './modules/learning/course-management/course-management.component';
import { CourseContentComponent } from './modules/learning/course-content/course-content.component';
import { LearningAssignmentComponent } from './modules/learning/learning-assignment/learning-assignment.component';
import { LearningProgressComponent } from './modules/learning/learning-progress/learning-progress.component';
import { AssessmentManagementComponent as LearningAssessmentManagementComponent } from './modules/learning/assessment-management/assessment-management.component';
import { AssessmentResultsComponent } from './modules/learning/assessment-results/assessment-results.component';
import { CertificationManagementComponent } from './modules/learning/certification-management/certification-management.component';
import { ComplianceManagementComponent } from './modules/learning/compliance-management/compliance-management.component';
import { SkillDevelopmentComponent } from './modules/learning/skill-development/skill-development.component';
import { LearningAnalyticsComponent } from './modules/learning/learning-analytics/learning-analytics.component';
import { MyLearningComponent } from './modules/learning/my-learning/my-learning.component';

import { TravelDashboardComponent } from './modules/travel/travel-dashboard/travel-dashboard.component';
import { TravelPolicyComponent } from './modules/travel/travel-policy/travel-policy.component';
import { TravelRequestComponent } from './modules/travel/travel-request/travel-request.component';
import { TravelApprovalComponent } from './modules/travel/travel-approval/travel-approval.component';
import { TravelAdvanceComponent } from './modules/travel/travel-advance/travel-advance.component';
import { ExpenseClaimComponent } from './modules/travel/expense-claim/expense-claim.component';
import { ExpenseApprovalComponent } from './modules/travel/expense-approval/expense-approval.component';
import { ExpenseSettlementComponent } from './modules/travel/expense-settlement/expense-settlement.component';
import { CorporateCardComponent } from './modules/travel/corporate-card/corporate-card.component';
import { TravelComplianceComponent } from './modules/travel/travel-compliance/travel-compliance.component';
import { TravelAnalyticsComponent } from './modules/travel/travel-analytics/travel-analytics.component';
import { MyTravelComponent } from './modules/travel/my-travel/my-travel.component';

import { AdminSetupComponent } from './modules/auth/admin-setup/admin-setup.component';
import { DataImportComponent } from './modules/data-import/data-import.component';

import { authGuard, permissionGuard, tenantResolver, userResolver, companyResolver } from './core/guards/guards';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'auth/setup-admin', component: AdminSetupComponent },
    { path: 'auth/forgot-password', component: ForgotPasswordComponent },
    { 
        path: '', 
        component: DashboardComponent,
        canActivate: [authGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardOverviewComponent },
            { path: 'data-import', component: DataImportComponent, canActivate: [permissionGuard], data: { permission: 'CONF_MANAGE' } },
            { path: 'tenants', component: TenantListComponent, resolve: { resolvedTenants: tenantResolver }, canActivate: [permissionGuard], data: { permission: 'TENANT_MANAGE' } },
            { path: 'companies', component: CompanyListComponent, resolve: { resolvedCompanies: companyResolver }, canActivate: [permissionGuard], data: { permission: 'ORG_MANAGE' } },
            { path: 'business-units', component: BusinessUnitListComponent, canActivate: [permissionGuard], data: { permission: 'ORG_MANAGE' } },
            { path: 'departments', component: DepartmentListComponent, canActivate: [permissionGuard], data: { permission: 'ORG_MANAGE' } },
            { path: 'designations', component: DesignationListComponent, canActivate: [permissionGuard], data: { permission: 'ORG_MANAGE' } },
            { path: 'locations', component: LocationListComponent, canActivate: [permissionGuard], data: { permission: 'ORG_MANAGE' } },
            { path: 'cost-centers', component: CostCenterListComponent, canActivate: [permissionGuard], data: { permission: 'ORG_MANAGE' } },
            { path: 'users', component: UserListComponent, resolve: { resolvedUsers: userResolver }, canActivate: [permissionGuard], data: { permission: 'USER_MANAGE' } },
            { path: 'roles', component: RoleListComponent, canActivate: [permissionGuard], data: { permission: 'ROLE_MANAGE' } },
            { path: 'permissions', component: PermissionListComponent, canActivate: [permissionGuard], data: { permission: 'PERM_MANAGE' } },
            { path: 'configurations', component: ConfigurationListComponent, canActivate: [permissionGuard], data: { permission: 'CONF_MANAGE' } },
            { path: 'notifications', component: NotificationHistoryComponent },
            { path: 'notifications/templates', component: NotificationTemplatesComponent },
            { path: 'notifications/preferences', component: NotificationPreferencesComponent },
            { path: 'notifications/dashboard', component: NotificationDashboardComponent },
            { path: 'hr/employees', component: EmployeeListComponent, canActivate: [permissionGuard], data: { permission: 'EMP_MANAGE' } },
            { path: 'hr/employees/:id', component: EmployeeDetailComponent, canActivate: [permissionGuard], data: { permission: 'EMP_MANAGE' } },
            { path: 'hr/transfers', component: TransferListComponent, canActivate: [permissionGuard], data: { permission: 'TRANSFER_MANAGE' } },
            { path: 'hr/promotions', component: PromotionListComponent, canActivate: [permissionGuard], data: { permission: 'PROMOTION_MANAGE' } },
            { path: 'hr/self-service', component: SelfServiceProfileComponent },
            { path: 'time/shifts', component: ShiftManagementComponent, canActivate: [permissionGuard], data: { permission: 'ATT_MANAGE' } },
            { path: 'time/attendance', component: AttendanceManagementComponent, canActivate: [permissionGuard], data: { permission: 'ATT_VIEW' } },
            { path: 'time/leaves', component: LeaveManagementComponent, canActivate: [permissionGuard], data: { permission: 'LEAVE_MANAGE' } },
            { path: 'time/overtime', component: OvertimeManagementComponent, canActivate: [permissionGuard], data: { permission: 'OVERTIME_MANAGE' } },
            { path: 'time/self-service', component: SelfServiceTimeComponent },
            { path: 'onboarding', component: OnboardingWorkflowComponent },
            { path: 'offboarding', component: OffboardingWorkflowComponent },
            { path: 'assets/dashboard', component: AssetDashboardComponent },
            { path: 'assets/categories', component: AssetCategoryComponent },
            { path: 'assets/register', component: AssetRegisterComponent },
            { path: 'assets/assignments', component: AssetAssignmentComponent },
            { path: 'assets/transfers', component: AssetTransferComponent },
            { path: 'assets/maintenance', component: AssetMaintenanceComponent },
            { path: 'assets/repairs', component: AssetRepairComponent },
            { path: 'assets/warranties', component: AssetWarrantyComponent },
            { path: 'assets/depreciation', component: AssetDepreciationComponent },
            { path: 'assets/audits', component: AssetAuditComponent },
            { path: 'assets/disposals', component: AssetDisposalComponent },
            { path: 'assets/returns', component: AssetReturnComponent },
            { path: 'assets/inventory', component: AssetInventoryComponent },
            { path: 'payroll/dashboard', component: PayrollDashboardComponent },
            { path: 'payroll/calendars', component: PayrollCalendarComponent },
            { path: 'payroll/periods', component: PayrollPeriodComponent },
            { path: 'payroll/salary-structures', component: SalaryStructureComponent },
            { path: 'payroll/compensation', component: EmployeeCompensationComponent },
            { path: 'payroll/runs', component: PayrollRunComponent },
            { path: 'payroll/adjustments', component: PayrollAdjustmentComponent },
            { path: 'payroll/arrears', component: ArrearsComponent },
            { path: 'payroll/loans', component: LoansAdvancesComponent },
            { path: 'payroll/bonuses', component: BonusManagementComponent },
            { path: 'payroll/incentives', component: IncentiveManagementComponent },
            { path: 'payroll/tax', component: TaxManagementComponent },
            { path: 'payroll/payslips', component: PayslipManagementComponent },
            { path: 'payroll/bank-transfers', component: BankTransferComponent },
            { path: 'payroll/final-settlements', component: FinalSettlementComponent },
            { path: 'performance/dashboard', component: PerformanceDashboardComponent },
            { path: 'performance/cycles', component: PerformanceCycleComponent },
            { path: 'performance/goals', component: GoalManagementComponent },
            { path: 'performance/competencies', component: CompetencyFrameworkComponent },
            { path: 'performance/feedback', component: FeedbackManagementComponent },
            { path: 'performance/checkins', component: CheckinManagementComponent },
            { path: 'performance/self-assessments', component: AssessmentManagementComponent },
            { path: 'performance/calibrations', component: CalibrationManagementComponent },
            { path: 'performance/ratings', component: RatingManagementComponent },
            { path: 'performance/development-plans', component: DevelopmentPlanComponent },
            { path: 'performance/promotions', component: PromotionManagementComponent },
            { path: 'performance/succession', component: SuccessionManagementComponent },
            { path: 'learning/dashboard', component: LearningDashboardComponent },
            { path: 'learning/courses', component: CourseManagementComponent },
            { path: 'learning/course-content', component: CourseContentComponent },
            { path: 'learning/assignments', component: LearningAssignmentComponent },
            { path: 'learning/progress', component: LearningProgressComponent },
            { path: 'learning/assessments', component: LearningAssessmentManagementComponent },
            { path: 'learning/results', component: AssessmentResultsComponent },
            { path: 'learning/certifications', component: CertificationManagementComponent },
            { path: 'learning/compliance', component: ComplianceManagementComponent },
            { path: 'learning/skills', component: SkillDevelopmentComponent },
            { path: 'learning/analytics', component: LearningAnalyticsComponent },
            { path: 'learning/my-learning', component: MyLearningComponent },
            { path: 'travel/dashboard', component: TravelDashboardComponent },
            { path: 'travel/policies', component: TravelPolicyComponent },
            { path: 'travel/requests', component: TravelRequestComponent },
            { path: 'travel/approvals', component: TravelApprovalComponent },
            { path: 'travel/advances', component: TravelAdvanceComponent },
            { path: 'travel/expense-claims', component: ExpenseClaimComponent },
            { path: 'travel/expense-approvals', component: ExpenseApprovalComponent },
            { path: 'travel/settlements', component: ExpenseSettlementComponent },
            { path: 'travel/corporate-cards', component: CorporateCardComponent },
            { path: 'travel/compliance', component: TravelComplianceComponent },
            { path: 'travel/analytics', component: TravelAnalyticsComponent },
            { path: 'travel/my-travel', component: MyTravelComponent }
        ]
    },
    { path: '**', redirectTo: 'login' }
];

