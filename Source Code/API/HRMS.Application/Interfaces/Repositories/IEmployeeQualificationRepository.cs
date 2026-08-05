using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeQualificationRepository
    {
        Task<long> CreateAsync(CreateEmployeeQualificationRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeQualificationRequest request);
        Task<bool> DeleteAsync(long employeeQualificationId, long tenantId, long deletedBy);
        Task<IEnumerable<EmployeeQualificationDto>> GetByEmployeeIdAsync(long employeeId, long tenantId);
    }
}