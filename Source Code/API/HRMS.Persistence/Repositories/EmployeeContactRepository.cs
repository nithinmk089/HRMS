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
    public class EmployeeContactRepository : IEmployeeContactRepository
    {
        private readonly string _connectionString;
        public EmployeeContactRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeContactRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@ContactType", request.ContactType);
            p.Add("@ContactValue", request.ContactValue);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeContactID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeContact_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeContactID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeContactRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeContactID", request.EmployeeContactId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@ContactType", request.ContactType);
            p.Add("@ContactValue", request.ContactValue);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeContact_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeContactId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeContactID", employeeContactId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeContact_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeContactDto>> GetByEmployeeIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeContactDto>(
                "SELECT * FROM hr.EmployeeContact WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0",
                new { EmployeeID = employeeId, TenantID = tenantId }
            );
        }
    }
}