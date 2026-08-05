using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IPerformanceRepository
    {
        // Cycles
        Task<long> CreateCycleAsync(CreatePerformanceCycleRequest r);
        Task<bool> OpenCycleAsync(long id, long tenantId, long modifiedBy);
        Task<IEnumerable<PerformanceCycleDto>> GetCyclesAsync(long tenantId);

        // Goals
        Task<long> CreateGoalAsync(CreateGoalRequest r);
        Task<IEnumerable<GoalDto>> GetGoalsAsync(long tenantId, long? employeeId, long? cycleId);

        // Goal Progress
        Task<long> CreateGoalProgressAsync(CreateGoalProgressRequest r);
        Task<IEnumerable<GoalProgressDto>> GetGoalProgressAsync(long goalId, long tenantId);

        // Appraisal Templates
        Task<IEnumerable<AppraisalTemplateDto>> GetTemplatesAsync(long tenantId);

        // Competencies
        Task<IEnumerable<CompetencyFrameworkDto>> GetCompetencyFrameworksAsync(long tenantId);

        // Feedback
        Task<long> CreateFeedbackAsync(CreateFeedbackRequest r);
        Task<IEnumerable<FeedbackDto>> GetFeedbackAsync(long tenantId, long? employeeId);

        // Check-ins
        Task<long> CreateCheckInAsync(CreateCheckInRequest r);
        Task<IEnumerable<CheckInMeetingDto>> GetCheckInsAsync(long tenantId, long? employeeId);

        // Self Assessment
        Task<long> CreateSelfAssessmentAsync(CreateSelfAssessmentRequest r);
        Task<IEnumerable<SelfAssessmentDto>> GetSelfAssessmentsAsync(long tenantId);
    }
}
