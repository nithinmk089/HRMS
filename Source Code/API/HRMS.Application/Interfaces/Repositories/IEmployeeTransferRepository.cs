using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeTransferRepository
    {
        Task<long> CreateAsync(CreateEmployeeTransferRequest request);
        Task<bool> ApproveAsync(long employeeTransferId, long tenantId, long approvedBy);
        Task<bool> CompleteAsync(long employeeTransferId, long tenantId, long completedBy);
        Task<IEnumerable<EmployeeTransferDto>> GetTransferHistoryReportAsync(long tenantId, long? employeeId);
    }
}