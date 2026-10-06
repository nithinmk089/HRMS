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
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateUserRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var (hash, salt) = HRMS.Application.Common.PasswordHelper.HashPassword(
                string.IsNullOrWhiteSpace(request.Password) ? "Admin@123" : request.Password);

            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@UserName", request.UserName);
            p.Add("@Email", request.Email);
            p.Add("@PasswordHash", hash);
            p.Add("@PasswordSalt", salt);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@UserID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("security.usp_User_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@UserID");
        }

        public async Task<bool> UpdateAsync(UpdateUserRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", request.UserId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@Email", request.Email);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_User_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long userId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_User_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<ApplicationUserDto?> GetByIdAsync(long userId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<ApplicationUserDto>("security.usp_User_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ApplicationUserDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("security.usp_User_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<ApplicationUserDto>();
        }

        public async Task<bool> LockAsync(long userId, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_User_Lock", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> UnlockAsync(long userId, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_User_Unlock", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> ResetPasswordAsync(long userId, long tenantId, string passwordHash, string passwordSalt, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            p.Add("@NewPasswordHash", passwordHash);
            p.Add("@NewPasswordSalt", passwordSalt);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_User_ResetPassword", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> ChangePasswordAsync(long userId, long tenantId, string passwordHash, string passwordSalt, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            p.Add("@NewPasswordHash", passwordHash);
            p.Add("@NewPasswordSalt", passwordSalt);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_User_ChangePassword", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}
