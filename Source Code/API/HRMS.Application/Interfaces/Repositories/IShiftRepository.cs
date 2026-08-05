using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IShiftRepository
    {
        Task<long> CreateShiftAsync(CreateShiftRequest request);
        Task<bool> UpdateShiftAsync(UpdateShiftRequest request);
        Task<bool> DeleteShiftAsync(long shiftId, long tenantId, long deletedBy);
        Task<ShiftDto?> GetShiftByIdAsync(long shiftId, long tenantId);
        Task<IEnumerable<ShiftDto>> SearchShiftsAsync(long tenantId, string? searchTerm);
        Task<long> CreateShiftAssignmentAsync(CreateShiftAssignmentRequest request);
        Task<bool> UpdateShiftAssignmentAsync(long id, long tenantId, long shiftId, DateTime fromDate, DateTime? toDate, long modifiedBy);
        Task<bool> DeleteShiftAssignmentAsync(long id, long tenantId, long deletedBy);
    }
}
