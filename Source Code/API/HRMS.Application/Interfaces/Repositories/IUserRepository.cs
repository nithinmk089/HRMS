using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<long> CreateAsync(CreateUserRequest request);
        Task<bool> UpdateAsync(UpdateUserRequest request);
        Task<bool> DeleteAsync(long userId, long tenantId, long deletedBy);
        Task<ApplicationUserDto?> GetByIdAsync(long userId, long tenantId);
        Task<IEnumerable<ApplicationUserDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
        Task<bool> LockAsync(long userId, long tenantId, long modifiedBy);
        Task<bool> UnlockAsync(long userId, long tenantId, long modifiedBy);
        Task<bool> ResetPasswordAsync(long userId, long tenantId, string passwordHash, string passwordSalt, long modifiedBy);
        Task<bool> ChangePasswordAsync(long userId, long tenantId, string passwordHash, string passwordSalt, long modifiedBy);
    }
}
