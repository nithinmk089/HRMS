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
    public class CostCenterRepository : ICostCenterRepository
    {
        private readonly string _connectionString;
        public CostCenterRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateCostCenterRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@CostCenterCode", request.CostCenterCode);
            p.Add("@CostCenterName", request.CostCenterName);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@CostCenterID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("organization.usp_CostCenter_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@CostCenterID");
        }

        public async Task<bool> UpdateAsync(UpdateCostCenterRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@CostCenterID", request.CostCenterId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@CostCenterName", request.CostCenterName);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("organization.usp_CostCenter_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long costCenterId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@CostCenterID", costCenterId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("organization.usp_CostCenter_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<CostCenterDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("organization.usp_CostCenter_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<CostCenterDto>();
        }
    }
}
