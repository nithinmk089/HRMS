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
    public class EmployeeStatusHistoryRepository : IEmployeeStatusHistoryRepository
    {
        private readonly string _connectionString;
        public EmployeeStatusHistoryRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(long tenantId, long employeeId, string status, DateTime effectiveDate, string? reason, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@Status", status);
            p.Add("@EffectiveDate", effectiveDate);
            p.Add("@Reason", reason);
            p.Add("@CreatedBy", createdBy);
            p.Add("@EmployeeStatusHistoryID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeStatusHistory_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeStatusHistoryID");
        }

        public async Task<IEnumerable<EmployeeStatusHistoryDto>> SearchAsync(long tenantId, long employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            return await conn.QueryAsync<EmployeeStatusHistoryDto>("hr.usp_EmployeeStatusHistory_Search", p, commandType: CommandType.StoredProcedure);
        }
    }
}