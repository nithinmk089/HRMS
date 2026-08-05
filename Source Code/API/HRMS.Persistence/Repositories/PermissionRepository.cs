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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly string _connectionString;
        public PermissionRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreatePermissionRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@PermissionCode", request.PermissionCode);
            p.Add("@PermissionName", request.PermissionName);
            p.Add("@ModuleCode", request.ModuleCode);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@PermissionID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("security.usp_Permission_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PermissionID");
        }

        public async Task<bool> UpdateAsync(UpdatePermissionRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@PermissionID", request.PermissionId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@PermissionName", request.PermissionName);
            p.Add("@ModuleCode", request.ModuleCode);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Permission_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long permissionId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@PermissionID", permissionId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Permission_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<PermissionDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("security.usp_Permission_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<PermissionDto>();
        }

        public async Task<bool> AssignToRoleAsync(long tenantId, long roleId, long permissionId, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@RoleID", roleId);
            p.Add("@PermissionID", permissionId);
            p.Add("@CreatedBy", createdBy);
            var affected = await conn.ExecuteAsync("security.usp_Permission_AssignToRole", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> RemoveFromRoleAsync(long tenantId, long roleId, long permissionId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@RoleID", roleId);
            p.Add("@PermissionID", permissionId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Permission_RemoveFromRole", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> AssignToUserAsync(long tenantId, long userId, long permissionId, bool isAllowed, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@UserID", userId);
            p.Add("@PermissionID", permissionId);
            p.Add("@IsAllowed", isAllowed);
            p.Add("@CreatedBy", createdBy);
            var affected = await conn.ExecuteAsync("security.usp_Permission_AssignToUser", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> RemoveFromUserAsync(long tenantId, long userId, long permissionId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@UserID", userId);
            p.Add("@PermissionID", permissionId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Permission_RemoveFromUser", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}
