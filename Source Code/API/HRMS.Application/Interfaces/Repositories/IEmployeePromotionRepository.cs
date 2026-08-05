using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeePromotionRepository
    {
        Task<long> CreateAsync(CreateEmployeePromotionRequest request);
        Task<bool> ApproveAsync(long employeePromotionId, long tenantId, long approvedBy);
        Task<bool> CompleteAsync(long employeePromotionId, long tenantId, long completedBy);
        Task<IEnumerable<EmployeePromotionDto>> GetPromotionHistoryReportAsync(long tenantId, long? employeeId);
    }
}