using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<long> CreateAsync(CreateEmployeeRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeRequest request);
        Task<bool> DeleteAsync(long employeeId, long tenantId, long deletedBy);
        Task<EmployeeDto?> GetByIdAsync(long employeeId, long tenantId);
        Task<IEnumerable<EmployeeDirectoryDto>> SearchAsync(long tenantId, string? searchText, string? status, int page, int pageSize);
        Task<bool> ActivateAsync(long employeeId, long tenantId, long modifiedBy);
        Task<bool> SuspendAsync(long employeeId, long tenantId, string? reason, long modifiedBy);
        Task<bool> TerminateAsync(long employeeId, long tenantId, string? reason, long modifiedBy);
        Task<bool> RehireAsync(long employeeId, long tenantId, string? reason, long modifiedBy);
        Task<bool> UpdateContactDetailsAsync(long employeeId, long tenantId, string? personalEmail, string? mobileNumber, long modifiedBy);
        Task<bool> UpdateProfileAsync(long employeeId, long tenantId, string? preferredName, string? maritalStatus, long modifiedBy);
        Task<IEnumerable<EmployeeServiceHistoryReportDto>> GetServiceHistoryReportAsync(long tenantId, long employeeId);
        Task<IEnumerable<EmployeeDirectoryDto>> GetDirectoryReportAsync(long tenantId, long? departmentId, long? locationId, string? status);
    }
}