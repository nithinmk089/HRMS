using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<long> CreateAsync(CreateCompanyRequest request);
        Task<bool> UpdateAsync(UpdateCompanyRequest request);
        Task<bool> DeleteAsync(long companyId, long tenantId, long deletedBy);
        Task<CompanyDto?> GetByIdAsync(long companyId, long tenantId);
        Task<IEnumerable<CompanyDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
    }
}
