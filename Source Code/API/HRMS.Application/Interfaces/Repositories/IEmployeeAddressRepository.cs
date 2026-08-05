using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeAddressRepository
    {
        Task<long> CreateAsync(CreateEmployeeAddressRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeAddressRequest request);
        Task<bool> DeleteAsync(long employeeAddressId, long tenantId, long deletedBy);
        Task<IEnumerable<EmployeeAddressDto>> GetByEmployeeIdAsync(long employeeId, long tenantId);
    }
}