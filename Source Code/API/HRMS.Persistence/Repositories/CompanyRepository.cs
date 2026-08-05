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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;
        public CompanyRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateCompanyRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@CompanyCode", request.CompanyCode);
            p.Add("@CompanyName", request.CompanyName);
            p.Add("@LegalName", request.LegalName);
            p.Add("@TaxNumber", request.TaxNumber);
            p.Add("@Email", request.Email);
            p.Add("@Phone", request.Phone);
            p.Add("@Website", request.Website);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@CompanyID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("security.usp_Company_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@CompanyID");
        }

        public async Task<bool> UpdateAsync(UpdateCompanyRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@CompanyID", request.CompanyId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@CompanyName", request.CompanyName);
            p.Add("@LegalName", request.LegalName);
            p.Add("@TaxNumber", request.TaxNumber);
            p.Add("@Email", request.Email);
            p.Add("@Phone", request.Phone);
            p.Add("@Website", request.Website);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("security.usp_Company_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long companyId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@CompanyID", companyId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("security.usp_Company_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<CompanyDto?> GetByIdAsync(long companyId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@CompanyID", companyId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<CompanyDto>("security.usp_Company_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CompanyDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("security.usp_Company_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<CompanyDto>();
        }
    }
}
