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
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly string _connectionString;
        public ConfigurationRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateConfigurationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@ConfigurationKey", request.ConfigurationKey);
            p.Add("@ConfigurationValue", request.ConfigurationValue);
            p.Add("@DataType", request.DataType);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ConfigurationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("system.usp_Configuration_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ConfigurationID");
        }

        public async Task<bool> UpdateAsync(UpdateConfigurationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ConfigurationID", request.ConfigurationId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@ConfigurationValue", request.ConfigurationValue);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("system.usp_Configuration_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long configurationId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ConfigurationID", configurationId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("system.usp_Configuration_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<ConfigurationDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("system.usp_Configuration_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<ConfigurationDto>();
        }

        public async Task<string?> GetValueAsync(long tenantId, string key)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = @"SELECT ConfigurationValue FROM system.Configuration WHERE TenantID = @TenantID AND ConfigurationKey = @Key AND IsDeleted = 0";
            return await conn.QueryFirstOrDefaultAsync<string?>(sql, new { TenantID = tenantId, Key = key });
        }

        public async Task<bool> SetValueAsync(long tenantId, string key, string value, string dataType = "String", long modifiedBy = 1)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = @"
                IF EXISTS (SELECT 1 FROM system.Configuration WHERE TenantID = @TenantID AND ConfigurationKey = @Key AND IsDeleted = 0)
                BEGIN
                    UPDATE system.Configuration
                    SET ConfigurationValue = @Value, DataType = @DataType, ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE(), VersionNo = VersionNo + 1
                    WHERE TenantID = @TenantID AND ConfigurationKey = @Key AND IsDeleted = 0;
                END
                ELSE
                BEGIN
                    INSERT INTO system.Configuration (TenantID, ConfigurationKey, ConfigurationValue, DataType, CreatedBy, CreatedDate, IsDeleted, VersionNo)
                    VALUES (@TenantID, @Key, @Value, @DataType, @ModifiedBy, GETUTCDATE(), 0, 1);
                END";
            var affected = await conn.ExecuteAsync(sql, new { TenantID = tenantId, Key = key, Value = value, DataType = dataType, ModifiedBy = modifiedBy });
            return affected > 0;
        }
    }
}
