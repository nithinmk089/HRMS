using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly string _connectionString;
        public PerformanceRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Cycles
        public async Task<long> CreateCycleAsync(CreatePerformanceCycleRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@CycleCode", r.CycleCode);
            p.Add("@CycleName", r.CycleName);
            p.Add("@StartDate", r.StartDate);
            p.Add("@EndDate", r.EndDate);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@PerformanceCycleID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("performance.usp_PerformanceCycle_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PerformanceCycleID");
        }

        public async Task<bool> OpenCycleAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("performance.usp_PerformanceCycle_Open", new { PerformanceCycleID = id, TenantID = tenantId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<PerformanceCycleDto>> GetCyclesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<PerformanceCycleDto>("SELECT * FROM performance.vw_PerformanceSummary WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        // Goals
        public async Task<long> CreateGoalAsync(CreateGoalRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@PerformanceCycleID", r.PerformanceCycleID);
            p.Add("@GoalTitle", r.GoalTitle);
            p.Add("@GoalDescription", r.GoalDescription);
            p.Add("@Weightage", r.Weightage);
            p.Add("@TargetValue", r.TargetValue);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@GoalID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("performance.usp_Goal_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@GoalID");
        }

        public async Task<IEnumerable<GoalDto>> GetGoalsAsync(long tenantId, long? employeeId, long? cycleId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM performance.vw_GoalProgress WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            if (cycleId.HasValue) sql += " AND PerformanceCycleID = @CycleID";
            return await conn.QueryAsync<GoalDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId, CycleID = cycleId });
        }

        // Goal Progress
        public async Task<long> CreateGoalProgressAsync(CreateGoalProgressRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@GoalID", r.GoalID);
            p.Add("@ProgressPercentage", r.ProgressPercentage);
            p.Add("@Remarks", r.Remarks);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@GoalProgressID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("performance.usp_GoalProgress_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@GoalProgressID");
        }

        public async Task<IEnumerable<GoalProgressDto>> GetGoalProgressAsync(long goalId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<GoalProgressDto>("SELECT * FROM performance.GoalProgress WHERE GoalID = @GoalID AND TenantID = @TenantID AND IsDeleted = 0", new { GoalID = goalId, TenantID = tenantId });
        }

        // Appraisal Templates
        public async Task<IEnumerable<AppraisalTemplateDto>> GetTemplatesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<AppraisalTemplateDto>("SELECT * FROM performance.AppraisalTemplate WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Competencies
        public async Task<IEnumerable<CompetencyFrameworkDto>> GetCompetencyFrameworksAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<CompetencyFrameworkDto>("SELECT * FROM performance.CompetencyFramework WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Feedback
        public async Task<long> CreateFeedbackAsync(CreateFeedbackRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO performance.Feedback (TenantID, EmployeeID, FeedbackText, CreatedBy) VALUES (@TenantID, @EmployeeID, @FeedbackText, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbackAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM performance.Feedback WHERE TenantID = @TenantID AND IsDeleted = 0";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<FeedbackDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Check-ins
        public async Task<long> CreateCheckInAsync(CreateCheckInRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO performance.CheckInMeeting (TenantID, EmployeeID, ManagerID, MeetingDate, Notes, CreatedBy) VALUES (@TenantID, @EmployeeID, @ManagerID, @MeetingDate, @Notes, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<CheckInMeetingDto>> GetCheckInsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM performance.CheckInMeeting WHERE TenantID = @TenantID AND IsDeleted = 0";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<CheckInMeetingDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Self Assessment
        public async Task<long> CreateSelfAssessmentAsync(CreateSelfAssessmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@PerformanceCycleID", r.PerformanceCycleID);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@SelfAssessmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("performance.usp_SelfAssessment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@SelfAssessmentID");
        }

        public async Task<IEnumerable<SelfAssessmentDto>> GetSelfAssessmentsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<SelfAssessmentDto>("SELECT * FROM performance.SelfAssessment WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }
    }
}
