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
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        public RoleRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateRoleRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@RoleCode", request.RoleCode);
            p.Add("@RoleName", request.RoleName);
            p.Add("@Description", request.Description);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@RoleID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("security.usp_Role_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@RoleID");
        }

        public async Task<bool> UpdateAsync(UpdateRoleRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@RoleID", request.RoleId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@RoleName", request.RoleName);
            p.Add("@Description", request.Description);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Role_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long roleId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@RoleID", roleId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Role_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<ApplicationRoleDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("security.usp_Role_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<ApplicationRoleDto>();
        }

        public async Task<IEnumerable<long>> GetUserIdsForRoleAsync(long roleId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = "SELECT UserID FROM security.vw_UserRoles WHERE RoleID = @RoleID AND TenantID = @TenantID;";
            return await conn.QueryAsync<long>(sql, new { RoleID = roleId, TenantID = tenantId });
        }

        public async Task<bool> AssignToUserAsync(long tenantId, long userId, long roleId, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@UserID", userId);
            p.Add("@RoleID", roleId);
            p.Add("@CreatedBy", createdBy);
            var affected = await conn.ExecuteAsync("security.usp_Role_AssignToUser", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> RemoveFromUserAsync(long tenantId, long userId, long roleId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@UserID", userId);
            p.Add("@RoleID", roleId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Role_RemoveFromUser", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}
