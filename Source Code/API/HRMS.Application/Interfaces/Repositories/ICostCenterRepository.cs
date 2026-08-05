using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ICostCenterRepository
    {
        Task<long> CreateAsync(CreateCostCenterRequest request);
        Task<bool> UpdateAsync(UpdateCostCenterRequest request);
        Task<bool> DeleteAsync(long costCenterId, long tenantId, long deletedBy);
        Task<IEnumerable<CostCenterDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
    }
}
