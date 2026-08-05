using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IDesignationRepository
    {
        Task<long> CreateAsync(CreateDesignationRequest request);
        Task<bool> UpdateAsync(UpdateDesignationRequest request);
        Task<bool> DeleteAsync(long designationId, long tenantId, long deletedBy);
        Task<IEnumerable<DesignationDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
    }
}
