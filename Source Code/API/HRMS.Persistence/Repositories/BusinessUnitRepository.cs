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
    public class BusinessUnitRepository : IBusinessUnitRepository
    {
        private readonly string _connectionString;
        public BusinessUnitRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateBusinessUnitRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@CompanyID", request.CompanyId);
            p.Add("@BusinessUnitCode", request.BusinessUnitCode);
            p.Add("@BusinessUnitName", request.BusinessUnitName);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@BusinessUnitID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("organization.usp_BusinessUnit_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@BusinessUnitID");
        }

        public async Task<bool> UpdateAsync(UpdateBusinessUnitRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@BusinessUnitID", request.BusinessUnitId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@BusinessUnitName", request.BusinessUnitName);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("organization.usp_BusinessUnit_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long businessUnitId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@BusinessUnitID", businessUnitId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("organization.usp_BusinessUnit_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<BusinessUnitDto>> SearchAsync(long tenantId, long? companyId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@CompanyID", companyId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("organization.usp_BusinessUnit_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<BusinessUnitDto>();
        }
    }
}
