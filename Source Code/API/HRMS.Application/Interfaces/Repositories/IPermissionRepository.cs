using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<long> CreateAsync(CreatePermissionRequest request);
        Task<bool> UpdateAsync(UpdatePermissionRequest request);
        Task<bool> DeleteAsync(long permissionId, long tenantId, long deletedBy);
        Task<IEnumerable<PermissionDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
        Task<bool> AssignToRoleAsync(long tenantId, long roleId, long permissionId, long createdBy);
        Task<bool> RemoveFromRoleAsync(long tenantId, long roleId, long permissionId, long deletedBy);
        Task<bool> AssignToUserAsync(long tenantId, long userId, long permissionId, bool isAllowed, long createdBy);
        Task<bool> RemoveFromUserAsync(long tenantId, long userId, long permissionId, long deletedBy);
    }
}
