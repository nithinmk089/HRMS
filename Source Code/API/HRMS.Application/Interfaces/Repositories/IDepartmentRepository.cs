using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<long> CreateAsync(CreateDepartmentRequest request);
        Task<bool> UpdateAsync(UpdateDepartmentRequest request);
        Task<bool> DeleteAsync(long departmentId, long tenantId, long deletedBy);
        Task<IEnumerable<DepartmentDto>> SearchAsync(long tenantId, long? businessUnitId, string? searchText, int page, int pageSize);
    }
}
