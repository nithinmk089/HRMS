using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeEmergencyContactRepository
    {
        Task<long> CreateAsync(CreateEmployeeEmergencyContactRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeEmergencyContactRequest request);
        Task<bool> DeleteAsync(long employeeEmergencyContactId, long tenantId, long deletedBy);
        Task<IEnumerable<EmployeeEmergencyContactDto>> GetByEmployeeIdAsync(long employeeId, long tenantId);
    }
}