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
    public class TaxRepository : ITaxRepository
    {
        private readonly string _connectionString;
        public TaxRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Regimes
        public async Task<long> CreateTaxRegimeAsync(CreateTaxRegimeRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO tax.TaxRegime (TenantID, RegimeName, EffectiveFrom, CreatedBy) VALUES (@TenantID, @RegimeName, @EffectiveFrom, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<TaxRegimeDto>> GetTaxRegimesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<TaxRegimeDto>("SELECT * FROM tax.TaxRegime WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Slabs
        public async Task<long> CreateTaxSlabAsync(CreateTaxSlabRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO tax.TaxSlab (TenantID, TaxRegimeID, IncomeFrom, IncomeTo, TaxRate, CreatedBy) VALUES (@TenantID, @TaxRegimeID, @IncomeFrom, @IncomeTo, @TaxRate, @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<TaxSlabDto>> GetTaxSlabsAsync(long regimeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<TaxSlabDto>("SELECT * FROM tax.TaxSlab WHERE TaxRegimeID = @RegimeID AND TenantID = @TenantID AND IsDeleted = 0", new { RegimeID = regimeId, TenantID = tenantId });
        }

        // Declarations
        public async Task<long> CreateTaxDeclarationAsync(CreateTaxDeclarationRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@FinancialYear", r.FinancialYear);
            p.Add("@DeclaredAmount", r.DeclaredAmount);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@EmployeeTaxDeclarationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("tax.usp_TaxDeclaration_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeTaxDeclarationID");
        }

        public async Task<IEnumerable<EmployeeTaxDeclarationDto>> GetTaxDeclarationsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeTaxDeclarationDto>("SELECT * FROM tax.EmployeeTaxDeclaration WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Computations & Statutory Deductions
        public async Task<IEnumerable<EmployeeTaxComputationDto>> GetTaxComputationsAsync(long tenantId, string financialYear)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeTaxComputationDto>("SELECT * FROM tax.vw_TaxComputation WHERE TenantID = @TenantID AND FinancialYear = @Year", new { TenantID = tenantId, Year = financialYear });
        }

        public async Task<IEnumerable<StatutoryDeductionDto>> GetStatutoryDeductionsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<StatutoryDeductionDto>("SELECT * FROM tax.vw_StatutoryDeductions WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }
    }
}
