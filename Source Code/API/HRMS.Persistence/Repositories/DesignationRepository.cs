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
    public class DesignationRepository : IDesignationRepository
    {
        private readonly string _connectionString;
        public DesignationRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateDesignationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@DesignationCode", request.DesignationCode);
            p.Add("@DesignationName", request.DesignationName);
            p.Add("@Grade", request.Grade);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@DesignationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("organization.usp_Designation_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@DesignationID");
        }

        public async Task<bool> UpdateAsync(UpdateDesignationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@DesignationID", request.DesignationId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@DesignationName", request.DesignationName);
            p.Add("@Grade", request.Grade);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("organization.usp_Designation_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long designationId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@DesignationID", designationId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("organization.usp_Designation_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<DesignationDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("organization.usp_Designation_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<DesignationDto>();
        }
    }
}
