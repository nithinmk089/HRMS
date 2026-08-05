using System;

namespace HRMS.Application.DTOs
{
    // COURSE CATEGORY
    public class CourseCategoryDto
    {
        public long CourseCategoryID { get; set; }
        public long TenantID { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public long? ParentCategoryID { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateCourseCategoryRequest
    {
        public long TenantID { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long CreatedBy { get; set; }
    }

    // COURSE
    public class CourseDto
    {
        public long CourseID { get; set; }
        public long TenantID { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public int DurationMinutes { get; set; }
        public string CourseStatus { get; set; } = string.Empty;
    }

    public class CreateCourseRequest
    {
        public long TenantID { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public long CourseCategoryID { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public long CreatedBy { get; set; }
    }

    // ENROLLMENT
    public class EnrollmentDto
    {
        public long EnrollmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        public decimal CompletionPercentage { get; set; }
    }

    public class CreateEnrollmentRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CourseID { get; set; }
        public long CreatedBy { get; set; }
    }

    // LEARNING ASSIGNMENT
    public class LearningAssignmentDto
    {
        public long LearningAssignmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CourseID { get; set; }
        public string AssignmentType { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string AssignmentStatus { get; set; } = string.Empty;
    }

    public class CreateLearningAssignmentRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CourseID { get; set; }
        public string AssignmentType { get; set; } = "Mandatory";
        public DateTime? DueDate { get; set; }
        public long CreatedBy { get; set; }
    }

    // ASSESSMENT
    public class LearningAssessmentDto
    {
        public long AssessmentID { get; set; }
        public long TenantID { get; set; }
        public long CourseID { get; set; }
        public string AssessmentName { get; set; } = string.Empty;
        public decimal PassPercentage { get; set; }
    }

    public class CreateLearningAssessmentRequest
    {
        public long TenantID { get; set; }
        public long CourseID { get; set; }
        public string AssessmentName { get; set; } = string.Empty;
        public decimal PassPercentage { get; set; } = 60;
        public long CreatedBy { get; set; }
    }

    // ASSESSMENT RESULT
    public class AssessmentResultDto
    {
        public long AssessmentResultID { get; set; }
        public long TenantID { get; set; }
        public long AssessmentID { get; set; }
        public string? AssessmentName { get; set; }
        public long EmployeeID { get; set; }
        public decimal Score { get; set; }
        public string ResultStatus { get; set; } = string.Empty;
        public decimal PassPercentage { get; set; }
    }

    public class SubmitAssessmentResultRequest
    {
        public long TenantID { get; set; }
        public long AssessmentID { get; set; }
        public long EmployeeID { get; set; }
        public decimal Score { get; set; }
        public long CreatedBy { get; set; }
    }

    // CERTIFICATION
    public class LearningCertificationDto
    {
        public long LearningCertificationID { get; set; }
        public long TenantID { get; set; }
        public long CourseID { get; set; }
        public string CertificationName { get; set; } = string.Empty;
        public int ValidityMonths { get; set; }
    }

    public class CertificationRenewalDto
    {
        public long CertificationRenewalID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? CertificationName { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int ValidityMonths { get; set; }
    }

    // COMPLIANCE
    public class ComplianceTrainingDto
    {
        public long ComplianceTrainingID { get; set; }
        public long TenantID { get; set; }
        public long CourseID { get; set; }
        public string ComplianceType { get; set; } = string.Empty;
        public bool MandatoryFlag { get; set; }
    }

    public class ComplianceAcknowledgementDto
    {
        public long ComplianceAcknowledgementID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? ComplianceType { get; set; }
        public DateTime AcknowledgedDate { get; set; }
        public bool MandatoryFlag { get; set; }
    }

    public class RecordComplianceAckRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long ComplianceTrainingID { get; set; }
        public long CreatedBy { get; set; }
    }

    // SKILLS
    public class LearningSkillDto
    {
        public long SkillID { get; set; }
        public long TenantID { get; set; }
        public string SkillCode { get; set; } = string.Empty;
        public string SkillName { get; set; } = string.Empty;
        public string SkillCategory { get; set; } = string.Empty;
    }

    public class EmployeeSkillDto
    {
        public long EmployeeSkillID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? SkillCode { get; set; }
        public string? SkillName { get; set; }
        public string? SkillCategory { get; set; }
        public string SkillLevel { get; set; } = string.Empty;
    }

    // ANALYTICS
    public class LearningAnalyticsSummaryDto
    {
        public long LearningAnalyticsID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public int CompletedCourses { get; set; }
        public int ActiveCourses { get; set; }
        public int CertificationsEarned { get; set; }
    }
}
