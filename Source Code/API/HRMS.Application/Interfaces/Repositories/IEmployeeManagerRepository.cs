using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeManagerRepository
    {
        Task<bool> AssignAsync(AssignManagerRequest request);
        Task<bool> RemoveAsync(long employeeId, long managerId, long tenantId, long deletedBy);
        Task<IEnumerable<EmployeeManagerDto>> GetManagersAsync(long employeeId, long tenantId);
        Task<IEnumerable<EmployeeDirectoryDto>> GetDirectReportsAsync(long managerId, long tenantId);
    }
}