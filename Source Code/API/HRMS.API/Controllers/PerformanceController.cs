using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/performance")]
    [ApiController]
    [Authorize]
    public class PerformanceController : ControllerBase
    {
        private readonly IPerformanceRepository _performanceRepository;
        public PerformanceController(IPerformanceRepository performanceRepository) => _performanceRepository = performanceRepository;

        // --- Cycles ---
        [HttpGet("cycles")]
        public async Task<IActionResult> GetCycles([FromQuery] long tenantId)
        {
            var res = await _performanceRepository.GetCyclesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<PerformanceCycleDto>>.SuccessResult(res));
        }

        [HttpPost("cycles")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateCycle([FromBody] CreatePerformanceCycleRequest r)
        {
            r.CreatedBy = 1;
            var id = await _performanceRepository.CreateCycleAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Performance appraisal cycle created."));
        }

        [HttpPost("cycles/{id}/open")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> OpenCycle(long id, [FromQuery] long tenantId)
        {
            var ok = await _performanceRepository.OpenCycleAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Cycle opened successfully."));
        }

        // --- Goals ---
        [HttpGet("goals")]
        public async Task<IActionResult> GetGoals([FromQuery] long tenantId, [FromQuery] long? employeeId, [FromQuery] long? cycleId)
        {
            var res = await _performanceRepository.GetGoalsAsync(tenantId, employeeId, cycleId);
            return Ok(ApiResponse<IEnumerable<GoalDto>>.SuccessResult(res));
        }

        [HttpPost("goals")]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest r)
        {
            r.CreatedBy = 1;
            var id = await _performanceRepository.CreateGoalAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Goal set successfully."));
        }

        // --- Goal Progress ---
        [HttpPost("goals/{id}/progress")]
        public async Task<IActionResult> LogGoalProgress(long id, [FromBody] CreateGoalProgressRequest r)
        {
            r.GoalID = id;
            r.CreatedBy = 1;
            var pId = await _performanceRepository.CreateGoalProgressAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(pId, "Goal progress logged successfully."));
        }

        [HttpGet("goals/{id}/progress")]
        public async Task<IActionResult> GetGoalProgress(long id, [FromQuery] long tenantId)
        {
            var res = await _performanceRepository.GetGoalProgressAsync(id, tenantId);
            return Ok(ApiResponse<IEnumerable<GoalProgressDto>>.SuccessResult(res));
        }

        // --- Templates ---
        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates([FromQuery] long tenantId)
        {
            var res = await _performanceRepository.GetTemplatesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<AppraisalTemplateDto>>.SuccessResult(res));
        }

        // --- Competency Frameworks ---
        [HttpGet("competencies")]
        public async Task<IActionResult> GetCompetencyFrameworks([FromQuery] long tenantId)
        {
            var res = await _performanceRepository.GetCompetencyFrameworksAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<CompetencyFrameworkDto>>.SuccessResult(res));
        }

        // --- Feedback ---
        [HttpGet("feedback")]
        public async Task<IActionResult> GetFeedback([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _performanceRepository.GetFeedbackAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<FeedbackDto>>.SuccessResult(res));
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest r)
        {
            r.CreatedBy = 1;
            var id = await _performanceRepository.CreateFeedbackAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Continuous feedback logged."));
        }

        // --- Check-ins ---
        [HttpGet("checkins")]
        public async Task<IActionResult> GetCheckIns([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _performanceRepository.GetCheckInsAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<CheckInMeetingDto>>.SuccessResult(res));
        }

        [HttpPost("checkins")]
        public async Task<IActionResult> CreateCheckIn([FromBody] CreateCheckInRequest r)
        {
            r.CreatedBy = 1;
            var id = await _performanceRepository.CreateCheckInAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Check-in 1-on-1 scheduled."));
        }

        // --- Self Assessments ---
        [HttpGet("self-assessments")]
        public async Task<IActionResult> GetSelfAssessments([FromQuery] long tenantId)
        {
            var res = await _performanceRepository.GetSelfAssessmentsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<SelfAssessmentDto>>.SuccessResult(res));
        }

        [HttpPost("self-assessments")]
        public async Task<IActionResult> CreateSelfAssessment([FromBody] CreateSelfAssessmentRequest r)
        {
            r.CreatedBy = 1;
            var id = await _performanceRepository.CreateSelfAssessmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self assessment record generated."));
        }
    }
}
