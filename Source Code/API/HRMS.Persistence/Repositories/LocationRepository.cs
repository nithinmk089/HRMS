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
    public class LocationRepository : ILocationRepository
    {
        private readonly string _connectionString;
        public LocationRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateLocationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@LocationCode", request.LocationCode);
            p.Add("@LocationName", request.LocationName);
            p.Add("@CountryCode", request.CountryCode);
            p.Add("@StateCode", request.StateCode);
            p.Add("@City", request.City);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@LocationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("organization.usp_Location_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@LocationID");
        }

        public async Task<bool> UpdateAsync(UpdateLocationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@LocationID", request.LocationId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@LocationName", request.LocationName);
            p.Add("@CountryCode", request.CountryCode);
            p.Add("@StateCode", request.StateCode);
            p.Add("@City", request.City);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("organization.usp_Location_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long locationId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@LocationID", locationId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("organization.usp_Location_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<LocationDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("organization.usp_Location_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<LocationDto>();
        }
    }
}
