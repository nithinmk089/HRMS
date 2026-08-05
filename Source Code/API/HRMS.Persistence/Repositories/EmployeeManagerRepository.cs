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
    public class EmployeeManagerRepository : IEmployeeManagerRepository
    {
        private readonly string _connectionString;
        public EmployeeManagerRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<bool> AssignAsync(AssignManagerRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@ManagerID", request.ManagerId);
            p.Add("@EffectiveFrom", request.EffectiveFrom);
            p.Add("@CreatedBy", request.CreatedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeManager_Assign", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> RemoveAsync(long employeeId, long managerId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@ManagerID", managerId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeManager_Remove", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeManagerDto>> GetManagersAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            return await conn.QueryAsync<EmployeeManagerDto>("hr.usp_EmployeeManager_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmployeeDirectoryDto>> GetDirectReportsAsync(long managerId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeDirectoryDto>(
                "SELECT * FROM hr.fn_GetDirectReports(@ManagerID, @TenantID)",
                new { ManagerID = managerId, TenantID = tenantId }
            );
        }
    }
}