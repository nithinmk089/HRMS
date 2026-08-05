using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeEmploymentRepository
    {
        Task<long> CreateAsync(CreateEmployeeEmploymentRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeEmploymentRequest request);
        Task<EmployeeEmploymentDto?> GetByEmployeeIdAsync(long employeeId, long tenantId);
    }
}