using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeContactRepository
    {
        Task<long> CreateAsync(CreateEmployeeContactRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeContactRequest request);
        Task<bool> DeleteAsync(long employeeContactId, long tenantId, long deletedBy);
        Task<IEnumerable<EmployeeContactDto>> GetByEmployeeIdAsync(long employeeId, long tenantId);
    }
}