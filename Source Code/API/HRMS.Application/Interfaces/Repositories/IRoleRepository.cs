using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<long> CreateAsync(CreateRoleRequest request);
        Task<bool> UpdateAsync(UpdateRoleRequest request);
        Task<bool> DeleteAsync(long roleId, long tenantId, long deletedBy);
        Task<IEnumerable<ApplicationRoleDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
        Task<IEnumerable<long>> GetUserIdsForRoleAsync(long roleId, long tenantId);
        Task<bool> AssignToUserAsync(long tenantId, long userId, long roleId, long createdBy);
        Task<bool> RemoveFromUserAsync(long tenantId, long userId, long roleId, long deletedBy);
    }
}
