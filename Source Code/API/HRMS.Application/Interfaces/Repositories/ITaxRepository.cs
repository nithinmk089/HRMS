using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ITaxRepository
    {
        // Regimes
        Task<long> CreateTaxRegimeAsync(CreateTaxRegimeRequest r);
        Task<IEnumerable<TaxRegimeDto>> GetTaxRegimesAsync(long tenantId);

        // Slabs
        Task<long> CreateTaxSlabAsync(CreateTaxSlabRequest r);
        Task<IEnumerable<TaxSlabDto>> GetTaxSlabsAsync(long regimeId, long tenantId);

        // Declarations
        Task<long> CreateTaxDeclarationAsync(CreateTaxDeclarationRequest r);
        Task<IEnumerable<EmployeeTaxDeclarationDto>> GetTaxDeclarationsAsync(long tenantId);

        // Computations & Statutory Deductions
        Task<IEnumerable<EmployeeTaxComputationDto>> GetTaxComputationsAsync(long tenantId, string financialYear);
        Task<IEnumerable<StatutoryDeductionDto>> GetStatutoryDeductionsAsync(long tenantId);
    }
}
