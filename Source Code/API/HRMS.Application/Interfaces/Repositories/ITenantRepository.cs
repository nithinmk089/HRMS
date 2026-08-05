using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ITenantRepository
    {
        Task<long> CreateAsync(CreateTenantRequest request);
        Task<bool> UpdateAsync(UpdateTenantRequest request);
        Task<bool> DeleteAsync(long tenantId, long deletedBy);
        Task<TenantDto?> GetByIdAsync(long tenantId);
        Task<IEnumerable<TenantDto>> SearchAsync(string? searchText, string? status, int page, int pageSize);
        Task<bool> ActivateAsync(long tenantId, long modifiedBy);
        Task<bool> DeactivateAsync(long tenantId, long modifiedBy);
        Task<bool> SuspendAsync(long tenantId, long modifiedBy);
    }
}
