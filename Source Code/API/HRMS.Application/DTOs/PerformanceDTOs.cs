using System;

namespace HRMS.Application.DTOs
{
    // CYCLES
    public class PerformanceCycleDto
    {
        public long PerformanceCycleID { get; set; }
        public long TenantID { get; set; }
        public string CycleCode { get; set; } = string.Empty;
        public string CycleName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CycleStatus { get; set; } = string.Empty;
        public int TotalGoals { get; set; }
        public decimal AvgGoalAchievement { get; set; }
    }

    public class CreatePerformanceCycleRequest
    {
        public long TenantID { get; set; }
        public string CycleCode { get; set; } = string.Empty;
        public string CycleName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CreatedBy { get; set; }
    }

    // GOALS
    public class GoalDto
    {
        public long GoalID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public long PerformanceCycleID { get; set; }
        public string GoalTitle { get; set; } = string.Empty;
        public string? GoalDescription { get; set; }
        public decimal Weightage { get; set; }
        public decimal TargetValue { get; set; }
        public decimal AchievementValue { get; set; }
        public decimal AchievementPercentage { get; set; }
        public string GoalStatus { get; set; } = string.Empty;
    }

    public class CreateGoalRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PerformanceCycleID { get; set; }
        public string GoalTitle { get; set; } = string.Empty;
        public string? GoalDescription { get; set; }
        public decimal Weightage { get; set; }
        public decimal TargetValue { get; set; }
        public long CreatedBy { get; set; }
    }

    // GOAL PROGRESS
    public class GoalProgressDto
    {
        public long GoalProgressID { get; set; }
        public long TenantID { get; set; }
        public long GoalID { get; set; }
        public DateTime ProgressDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public string? Remarks { get; set; }
    }

    public class CreateGoalProgressRequest
    {
        public long TenantID { get; set; }
        public long GoalID { get; set; }
        public decimal ProgressPercentage { get; set; }
        public string? Remarks { get; set; }
        public long CreatedBy { get; set; }
    }

    // TEMPLATES
    public class AppraisalTemplateDto
    {
        public long AppraisalTemplateID { get; set; }
        public long TenantID { get; set; }
        public string TemplateCode { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    // COMPETENCY
    public class CompetencyFrameworkDto
    {
        public long CompetencyFrameworkID { get; set; }
        public long TenantID { get; set; }
        public string FrameworkName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // FEEDBACK
    public class FeedbackDto
    {
        public long FeedbackID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string FeedbackText { get; set; } = string.Empty;
    }

    public class CreateFeedbackRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string FeedbackText { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // CHECK-IN
    public class CheckInMeetingDto
    {
        public long CheckInMeetingID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public long ManagerID { get; set; }
        public string? ManagerName { get; set; }
        public DateTime MeetingDate { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateCheckInRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long ManagerID { get; set; }
        public DateTime MeetingDate { get; set; }
        public string? Notes { get; set; }
        public long CreatedBy { get; set; }
    }

    // SELF ASSESSMENT
    public class SelfAssessmentDto
    {
        public long SelfAssessmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public long PerformanceCycleID { get; set; }
        public string AssessmentStatus { get; set; } = string.Empty;
    }

    public class CreateSelfAssessmentRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PerformanceCycleID { get; set; }
        public long CreatedBy { get; set; }
    }

    // CALIBRATION
    public class CalibrationSessionDto
    {
        public long CalibrationSessionID { get; set; }
        public long TenantID { get; set; }
        public DateTime SessionDate { get; set; }
        public long FacilitatorID { get; set; }
        public string SessionStatus { get; set; } = string.Empty;
    }

    // RATINGS
    public class PerformanceRatingDto
    {
        public long PerformanceRatingID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public long PerformanceCycleID { get; set; }
        public decimal RatingValue { get; set; }
        public string? RatingRemarks { get; set; }
    }

    // DEVELOPMENT PLAN
    public class DevelopmentPlanDto
    {
        public long DevelopmentPlanID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string PlanTitle { get; set; } = string.Empty;
        public DateTime TargetCompletionDate { get; set; }
    }

    // PROMOTIONS
    public class PromotionRecommendationDto
    {
        public long PromotionRecommendationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string RecommendedRole { get; set; } = string.Empty;
        public string RecommendationStatus { get; set; } = string.Empty;
    }

    // SUCCESSION
    public class SuccessionInputDto
    {
        public long SuccessionInputID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string ReadinessLevel { get; set; } = string.Empty;
        public bool CriticalRoleFlag { get; set; }
    }
}
