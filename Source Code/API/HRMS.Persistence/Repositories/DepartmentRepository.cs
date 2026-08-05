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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;
        public DepartmentRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateDepartmentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@BusinessUnitID", request.BusinessUnitId);
            p.Add("@DepartmentCode", request.DepartmentCode);
            p.Add("@DepartmentName", request.DepartmentName);
            p.Add("@ParentDepartmentID", request.ParentDepartmentId);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@DepartmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("organization.usp_Department_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@DepartmentID");
        }

        public async Task<bool> UpdateAsync(UpdateDepartmentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@DepartmentID", request.DepartmentId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@DepartmentName", request.DepartmentName);
            p.Add("@ParentDepartmentID", request.ParentDepartmentId);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("organization.usp_Department_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long departmentId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@DepartmentID", departmentId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("organization.usp_Department_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<DepartmentDto>> SearchAsync(long tenantId, long? businessUnitId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@BusinessUnitID", businessUnitId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("organization.usp_Department_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<DepartmentDto>();
        }
    }
}
