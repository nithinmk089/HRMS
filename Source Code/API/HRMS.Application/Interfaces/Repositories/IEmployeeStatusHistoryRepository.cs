using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeStatusHistoryRepository
    {
        Task<long> CreateAsync(long tenantId, long employeeId, string status, DateTime effectiveDate, string? reason, long createdBy);
        Task<IEnumerable<EmployeeStatusHistoryDto>> SearchAsync(long tenantId, long employeeId);
    }
}