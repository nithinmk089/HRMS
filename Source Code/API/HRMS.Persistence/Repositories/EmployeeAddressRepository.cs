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
    public class EmployeeAddressRepository : IEmployeeAddressRepository
    {
        private readonly string _connectionString;
        public EmployeeAddressRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeAddressRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@AddressType", request.AddressType);
            p.Add("@AddressLine1", request.AddressLine1);
            p.Add("@AddressLine2", request.AddressLine2);
            p.Add("@City", request.City);
            p.Add("@State", request.State);
            p.Add("@Country", request.Country);
            p.Add("@ZipCode", request.ZipCode);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeAddressID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeAddress_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeAddressID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeAddressRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeAddressID", request.EmployeeAddressId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@AddressType", request.AddressType);
            p.Add("@AddressLine1", request.AddressLine1);
            p.Add("@AddressLine2", request.AddressLine2);
            p.Add("@City", request.City);
            p.Add("@State", request.State);
            p.Add("@Country", request.Country);
            p.Add("@ZipCode", request.ZipCode);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeAddress_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeAddressId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeAddressID", employeeAddressId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeAddress_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeAddressDto>> GetByEmployeeIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeAddressDto>(
                "SELECT * FROM hr.EmployeeAddress WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0",
                new { EmployeeID = employeeId, TenantID = tenantId }
            );
        }
    }
}