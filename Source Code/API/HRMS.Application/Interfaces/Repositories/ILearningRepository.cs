using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ILearningRepository
    {
        // Courses
        Task<long> CreateCourseAsync(CreateCourseRequest r);
        Task<IEnumerable<CourseDto>> GetCoursesAsync(long tenantId);
        Task<IEnumerable<CourseCategoryDto>> GetCategoriesAsync(long tenantId);
        Task<long> CreateCategoryAsync(CreateCourseCategoryRequest r);

        // Enrollments
        Task<long> CreateEnrollmentAsync(CreateEnrollmentRequest r);
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsAsync(long tenantId, long? employeeId);

        // Assignments
        Task<long> CreateAssignmentAsync(CreateLearningAssignmentRequest r);
        Task<IEnumerable<LearningAssignmentDto>> GetAssignmentsAsync(long tenantId, long? employeeId);

        // Assessments
        Task<long> CreateAssessmentAsync(CreateLearningAssessmentRequest r);
        Task<IEnumerable<LearningAssessmentDto>> GetAssessmentsAsync(long tenantId);
        Task<long> SubmitAssessmentResultAsync(SubmitAssessmentResultRequest r);
        Task<IEnumerable<AssessmentResultDto>> GetAssessmentResultsAsync(long tenantId, long? employeeId);

        // Certifications
        Task<IEnumerable<LearningCertificationDto>> GetCertificationsAsync(long tenantId);
        Task<IEnumerable<CertificationRenewalDto>> GetCertificationRenewalsAsync(long tenantId, long? employeeId);

        // Compliance
        Task<IEnumerable<ComplianceTrainingDto>> GetComplianceTrainingAsync(long tenantId);
        Task<long> RecordComplianceAckAsync(RecordComplianceAckRequest r);
        Task<IEnumerable<ComplianceAcknowledgementDto>> GetComplianceAcksAsync(long tenantId, long? employeeId);

        // Skills
        Task<IEnumerable<LearningSkillDto>> GetSkillsAsync(long tenantId);
        Task<IEnumerable<EmployeeSkillDto>> GetEmployeeSkillsAsync(long tenantId, long? employeeId);

        // Analytics
        Task<IEnumerable<LearningAnalyticsSummaryDto>> GetAnalyticsSummaryAsync(long tenantId);
    }
}
