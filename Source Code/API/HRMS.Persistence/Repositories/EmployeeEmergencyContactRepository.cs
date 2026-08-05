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
    public class EmployeeEmergencyContactRepository : IEmployeeEmergencyContactRepository
    {
        private readonly string _connectionString;
        public EmployeeEmergencyContactRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeEmergencyContactRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@ContactName", request.ContactName);
            p.Add("@Relationship", request.Relationship);
            p.Add("@MobileNumber", request.MobileNumber);
            p.Add("@Email", request.Email);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeEmergencyContactID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeEmergencyContact_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeEmergencyContactID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeEmergencyContactRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeEmergencyContactID", request.EmployeeEmergencyContactId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@ContactName", request.ContactName);
            p.Add("@Relationship", request.Relationship);
            p.Add("@MobileNumber", request.MobileNumber);
            p.Add("@Email", request.Email);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeEmergencyContact_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeEmergencyContactId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeEmergencyContactID", employeeEmergencyContactId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeEmergencyContact_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeEmergencyContactDto>> GetByEmployeeIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeEmergencyContactDto>(
                "SELECT * FROM hr.EmployeeEmergencyContact WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0",
                new { EmployeeID = employeeId, TenantID = tenantId }
            );
        }
    }
}