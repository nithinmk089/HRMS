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
    public class LearningRepository : ILearningRepository
    {
        private readonly string _connectionString;
        public LearningRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Courses
        public async Task<long> CreateCourseAsync(CreateCourseRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID); p.Add("@CourseCode", r.CourseCode); p.Add("@CourseName", r.CourseName);
            p.Add("@CourseCategoryID", r.CourseCategoryID); p.Add("@Description", r.Description);
            p.Add("@DurationMinutes", r.DurationMinutes); p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@CourseID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("learning.usp_Course_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@CourseID");
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<CourseDto>("SELECT * FROM learning.vw_CourseCatalog WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<CourseCategoryDto>> GetCategoriesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<CourseCategoryDto>("SELECT * FROM learning.CourseCategory WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<long> CreateCategoryAsync(CreateCourseCategoryRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO learning.CourseCategory (TenantID, CategoryCode, CategoryName, Description, CreatedBy) VALUES (@TenantID, @CategoryCode, @CategoryName, @Description, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        // Enrollments
        public async Task<long> CreateEnrollmentAsync(CreateEnrollmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID); p.Add("@EmployeeID", r.EmployeeID); p.Add("@CourseID", r.CourseID);
            p.Add("@CreatedBy", r.CreatedBy); p.Add("@EnrollmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("learning.usp_Enrollment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EnrollmentID");
        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM learning.vw_LearningProgress WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<EnrollmentDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Assignments
        public async Task<long> CreateAssignmentAsync(CreateLearningAssignmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO learning.LearningAssignment (TenantID, EmployeeID, CourseID, AssignmentType, DueDate, CreatedBy) VALUES (@TenantID, @EmployeeID, @CourseID, @AssignmentType, @DueDate, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<LearningAssignmentDto>> GetAssignmentsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"SELECT a.LearningAssignmentID, a.TenantID, a.EmployeeID, CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
                               a.CourseID, c.CourseName, a.AssignmentType, a.DueDate, a.AssignmentStatus
                        FROM learning.LearningAssignment a
                        LEFT JOIN hr.Employee e ON a.EmployeeID = e.EmployeeID
                        LEFT JOIN learning.Course c ON a.CourseID = c.CourseID
                        WHERE a.TenantID = @TenantID AND a.IsDeleted = 0";
            if (employeeId.HasValue) sql += " AND a.EmployeeID = @EmployeeID";
            return await conn.QueryAsync<LearningAssignmentDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Assessments
        public async Task<long> CreateAssessmentAsync(CreateLearningAssessmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO learning.Assessment (TenantID, CourseID, AssessmentName, PassPercentage, CreatedBy) VALUES (@TenantID, @CourseID, @AssessmentName, @PassPercentage, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<LearningAssessmentDto>> GetAssessmentsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<LearningAssessmentDto>("SELECT * FROM learning.Assessment WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<long> SubmitAssessmentResultAsync(SubmitAssessmentResultRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID); p.Add("@AssessmentID", r.AssessmentID); p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@Score", r.Score); p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@AssessmentResultID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("learning.usp_AssessmentResult_Submit", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AssessmentResultID");
        }

        public async Task<IEnumerable<AssessmentResultDto>> GetAssessmentResultsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM learning.vw_AssessmentResults WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<AssessmentResultDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Certifications
        public async Task<IEnumerable<LearningCertificationDto>> GetCertificationsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<LearningCertificationDto>("SELECT * FROM learning.LearningCertification WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<CertificationRenewalDto>> GetCertificationRenewalsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM learning.vw_CertificationStatus WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<CertificationRenewalDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Compliance
        public async Task<IEnumerable<ComplianceTrainingDto>> GetComplianceTrainingAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<ComplianceTrainingDto>("SELECT * FROM learning.ComplianceTraining WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<long> RecordComplianceAckAsync(RecordComplianceAckRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID); p.Add("@EmployeeID", r.EmployeeID); p.Add("@ComplianceTrainingID", r.ComplianceTrainingID);
            p.Add("@CreatedBy", r.CreatedBy); p.Add("@ComplianceAcknowledgementID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("learning.usp_ComplianceAcknowledgement_Record", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ComplianceAcknowledgementID");
        }

        public async Task<IEnumerable<ComplianceAcknowledgementDto>> GetComplianceAcksAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM learning.vw_ComplianceStatus WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<ComplianceAcknowledgementDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Skills
        public async Task<IEnumerable<LearningSkillDto>> GetSkillsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<LearningSkillDto>("SELECT * FROM learning.Skill WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<EmployeeSkillDto>> GetEmployeeSkillsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM learning.vw_SkillGapAnalysis WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            return await conn.QueryAsync<EmployeeSkillDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId });
        }

        // Analytics
        public async Task<IEnumerable<LearningAnalyticsSummaryDto>> GetAnalyticsSummaryAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<LearningAnalyticsSummaryDto>("SELECT * FROM learning.LearningAnalytics WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }
    }
}
