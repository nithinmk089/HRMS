using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/learning")]
    [Authorize]
    public class LearningController : BaseApiController
    {
        private readonly ILearningRepository _learningRepository;
        public LearningController(ILearningRepository learningRepository) => _learningRepository = learningRepository;

        // --- Courses ---
        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetCoursesAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResult(res));
        }

        [HttpPost("courses")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.CreateCourseAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Course created."));
        }

        // --- Categories ---
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetCategoriesAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<CourseCategoryDto>>.SuccessResult(res));
        }

        [HttpPost("categories")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCourseCategoryRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.CreateCategoryAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Category created."));
        }

        // --- Enrollments ---
        [HttpGet("enrollments")]
        public async Task<IActionResult> GetEnrollments([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetEnrollmentsAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<EnrollmentDto>>.SuccessResult(res));
        }

        [HttpPost("enrollments")]
        public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.CreateEnrollmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Enrollment created."));
        }

        // --- Assignments ---
        [HttpGet("assignments")]
        public async Task<IActionResult> GetAssignments([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetAssignmentsAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<LearningAssignmentDto>>.SuccessResult(res));
        }

        [HttpPost("assignments")]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateLearningAssignmentRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.CreateAssignmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Learning assignment created."));
        }

        // --- Assessments ---
        [HttpGet("assessments")]
        public async Task<IActionResult> GetAssessments([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetAssessmentsAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<LearningAssessmentDto>>.SuccessResult(res));
        }

        [HttpPost("assessments")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateAssessment([FromBody] CreateLearningAssessmentRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.CreateAssessmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Assessment created."));
        }

        // --- Assessment Results ---
        [HttpGet("assessment-results")]
        public async Task<IActionResult> GetAssessmentResults([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetAssessmentResultsAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<AssessmentResultDto>>.SuccessResult(res));
        }

        [HttpPost("assessment-results")]
        public async Task<IActionResult> SubmitAssessmentResult([FromBody] SubmitAssessmentResultRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.SubmitAssessmentResultAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Assessment result submitted."));
        }

        // --- Certifications ---
        [HttpGet("certifications")]
        public async Task<IActionResult> GetCertifications([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetCertificationsAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<LearningCertificationDto>>.SuccessResult(res));
        }

        [HttpGet("certification-renewals")]
        public async Task<IActionResult> GetCertificationRenewals([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetCertificationRenewalsAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<CertificationRenewalDto>>.SuccessResult(res));
        }

        // --- Compliance ---
        [HttpGet("compliance-training")]
        public async Task<IActionResult> GetComplianceTraining([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetComplianceTrainingAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<ComplianceTrainingDto>>.SuccessResult(res));
        }

        [HttpPost("compliance-acknowledgements")]
        public async Task<IActionResult> RecordComplianceAck([FromBody] RecordComplianceAckRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _learningRepository.RecordComplianceAckAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Compliance acknowledged."));
        }

        [HttpGet("compliance-acknowledgements")]
        public async Task<IActionResult> GetComplianceAcks([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetComplianceAcksAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<ComplianceAcknowledgementDto>>.SuccessResult(res));
        }

        // --- Skills ---
        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetSkillsAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<LearningSkillDto>>.SuccessResult(res));
        }

        [HttpGet("employee-skills")]
        public async Task<IActionResult> GetEmployeeSkills([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetEmployeeSkillsAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<EmployeeSkillDto>>.SuccessResult(res));
        }

        // --- Analytics ---
        [HttpGet("analytics/summary")]
        public async Task<IActionResult> GetAnalyticsSummary([FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _learningRepository.GetAnalyticsSummaryAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<LearningAnalyticsSummaryDto>>.SuccessResult(res));
        }
    }
}
