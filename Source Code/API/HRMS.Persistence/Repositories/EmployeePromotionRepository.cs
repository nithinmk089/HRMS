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
    public class EmployeePromotionRepository : IEmployeePromotionRepository
    {
        private readonly string _connectionString;
        public EmployeePromotionRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeePromotionRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@OldDesignationID", request.OldDesignationId);
            p.Add("@NewDesignationID", request.NewDesignationId);
            p.Add("@OldGrade", request.OldGrade);
            p.Add("@NewGrade", request.NewGrade);
            p.Add("@EffectiveDate", request.EffectiveDate);
            p.Add("@Reason", request.Reason);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeePromotionID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeePromotion_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeePromotionID");
        }

        public async Task<bool> ApproveAsync(long employeePromotionId, long tenantId, long approvedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeePromotionID", employeePromotionId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", approvedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeePromotion_Approve", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CompleteAsync(long employeePromotionId, long tenantId, long completedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeePromotionID", employeePromotionId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", completedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeePromotion_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeePromotionDto>> GetPromotionHistoryReportAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            return await conn.QueryAsync<EmployeePromotionDto>("hr.usp_Report_EmployeePromotions", p, commandType: CommandType.StoredProcedure);
        }
    }
}