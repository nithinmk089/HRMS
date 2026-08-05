using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ILeaveRepository
    {
        Task<long> CreateLeaveTypeAsync(CreateLeaveTypeRequest request);
        Task<IEnumerable<LeaveTypeDto>> SearchLeaveTypesAsync(long tenantId, string? searchTerm);
        Task<long> CreateLeavePolicyAsync(CreateLeavePolicyRequest request);
        Task<IEnumerable<LeaveBalanceDto>> GetBalancesAsync(long tenantId, long employeeId, long? leaveTypeId);
        Task<long> CreateLeaveRequestAsync(CreateLeaveRequestRequest request);
        Task<IEnumerable<LeaveRequestDto>> SearchLeaveRequestsAsync(long tenantId, long? employeeId, string? status);
        Task<bool> ApproveLeaveRequestAsync(long id, long tenantId, long approverId, string? remarks);
        Task<bool> RejectLeaveRequestAsync(long id, long tenantId, long approverId, string? remarks);
        Task<bool> CancelLeaveRequestAsync(long id, long tenantId, long modifiedBy);
        Task<long> CreateLeaveEncashmentAsync(CreateLeaveEncashmentRequest request);
        Task<bool> ApproveLeaveEncashmentAsync(long id, long tenantId, long approverId);
    }
}
