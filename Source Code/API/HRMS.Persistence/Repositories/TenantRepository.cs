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
    public class TenantRepository : ITenantRepository
    {
        private readonly string _connectionString;
        public TenantRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateTenantRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantCode", request.TenantCode);
            p.Add("@TenantName", request.TenantName);
            p.Add("@Status", request.Status);
            p.Add("@EffectiveFrom", request.EffectiveFrom);
            p.Add("@EffectiveTo", request.EffectiveTo);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@TenantID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("security.usp_Tenant_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@TenantID");
        }

        public async Task<bool> UpdateAsync(UpdateTenantRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@TenantName", request.TenantName);
            p.Add("@Status", request.Status);
            p.Add("@EffectiveFrom", request.EffectiveFrom);
            p.Add("@EffectiveTo", request.EffectiveTo);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Tenant_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Tenant_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<TenantDto?> GetByIdAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<TenantDto>("security.usp_Tenant_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TenantDto>> SearchAsync(string? searchText, string? status, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@SearchText", searchText);
            p.Add("@Status", status);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("security.usp_Tenant_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<TenantDto>();
        }

        public async Task<bool> ActivateAsync(long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Tenant_Activate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeactivateAsync(long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Tenant_Deactivate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> SuspendAsync(long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Tenant_Suspend", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}
