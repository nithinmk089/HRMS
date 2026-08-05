using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task<long> ClockInAsync(CreateAttendanceRequest request);
        Task<bool> ClockOutAsync(UpdateAttendanceRequest request);
        Task<AttendanceDto?> GetByIdAsync(long attendanceId, long tenantId);
        Task<IEnumerable<AttendanceDto>> SearchAsync(long tenantId, long? employeeId, DateTime? startDate, DateTime? endDate);
        Task<bool> RecalculateAsync(long tenantId, long employeeId, DateTime attendanceDate, long modifiedBy);
        Task<long> CreateAdjustmentAsync(CreateAttendanceAdjustmentRequest request);
        Task<bool> ApproveAdjustmentAsync(long id, long tenantId, long approverId);
        Task<bool> RejectAdjustmentAsync(long id, long tenantId, long approverId);
        Task<long> CreateRegularizationAsync(CreateAttendanceRegularizationRequest request);
        Task<bool> ApproveRegularizationAsync(long id, long tenantId, long approverId);
        Task<bool> RejectRegularizationAsync(long id, long tenantId, long approverId);
    }
}
