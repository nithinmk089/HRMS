using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IBusinessUnitRepository
    {
        Task<long> CreateAsync(CreateBusinessUnitRequest request);
        Task<bool> UpdateAsync(UpdateBusinessUnitRequest request);
        Task<bool> DeleteAsync(long businessUnitId, long tenantId, long deletedBy);
        Task<IEnumerable<BusinessUnitDto>> SearchAsync(long tenantId, long? companyId, string? searchText, int page, int pageSize);
    }
}
