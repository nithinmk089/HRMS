using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IOvertimeRepository
    {
        Task<long> CreateOvertimeRequestAsync(CreateOvertimeRequestRequest request);
        Task<IEnumerable<OvertimeRequestDto>> SearchOvertimeRequestsAsync(long tenantId, long? employeeId, string? status);
        Task<bool> ApproveOvertimeRequestAsync(long id, long tenantId, long approverId, string? remarks);
        Task<bool> RejectOvertimeRequestAsync(long id, long tenantId, long approverId, string? remarks);
    }
}
