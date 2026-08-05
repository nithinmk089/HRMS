using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeCertificationRepository
    {
        Task<long> CreateAsync(CreateEmployeeCertificationRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeCertificationRequest request);
        Task<bool> DeleteAsync(long employeeCertificationId, long tenantId, long deletedBy);
        Task<IEnumerable<EmployeeCertificationDto>> GetByEmployeeIdAsync(long employeeId, long tenantId);
        Task<IEnumerable<CertificationExpiryReportDto>> GetCertificationExpiryReportAsync(long tenantId, int withinDays);
    }
}