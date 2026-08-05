using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IPayrollRepository
    {
        // Calendars
        Task<long> CreateCalendarAsync(CreatePayrollCalendarRequest r);
        Task<bool> UpdateCalendarAsync(UpdatePayrollCalendarRequest r);
        Task<bool> DeleteCalendarAsync(long id, long tenantId, long deletedBy);
        Task<IEnumerable<PayrollCalendarDto>> SearchCalendarsAsync(long tenantId, string? searchText, int page, int pageSize);

        // Periods
        Task<long> CreatePeriodAsync(CreatePayrollPeriodRequest r);
        Task<bool> OpenPeriodAsync(long id, long tenantId, long modifiedBy);
        Task<bool> ClosePeriodAsync(long id, long tenantId, long modifiedBy);
        Task<bool> LockPeriodAsync(long id, long tenantId, long modifiedBy);
        Task<IEnumerable<PayrollPeriodDto>> GetPeriodsAsync(long tenantId, long calendarId);

        // Salary Structures
        Task<long> CreateSalaryStructureAsync(CreateSalaryStructureRequest r);
        Task<bool> UpdateSalaryStructureAsync(UpdateSalaryStructureRequest r);
        Task<IEnumerable<SalaryStructureDto>> GetSalaryStructuresAsync(long tenantId);

        // Salary Components
        Task<long> CreateSalaryComponentAsync(CreateSalaryComponentRequest r);
        Task<IEnumerable<SalaryComponentDto>> GetSalaryComponentsAsync(long tenantId);

        // Employee Compensation
        Task<long> CreateEmployeeCompensationAsync(CreateEmployeeCompensationRequest r);
        Task<IEnumerable<EmployeeCompensationDto>> GetEmployeeCompensationsAsync(long tenantId);

        // Payroll Run
        Task<long> CreatePayrollRunAsync(CreatePayrollRunRequest r);
        Task<bool> ProcessPayrollRunAsync(long id, long tenantId, long modifiedBy);
        Task<IEnumerable<PayrollRunDto>> GetPayrollRunsAsync(long tenantId);
        Task<IEnumerable<PayrollTransactionDto>> GetPayrollTransactionsAsync(long runId, long tenantId);

        // Adjustments
        Task<long> CreateAdjustmentAsync(CreatePayrollAdjustmentRequest r);
        Task<bool> ApproveAdjustmentAsync(long id, long tenantId, long approvedBy);
        Task<IEnumerable<PayrollAdjustmentDto>> GetAdjustmentsAsync(long tenantId);

        // Loans & Advances
        Task<long> CreateLoanAdvanceAsync(CreateLoanRequest r);
        Task<bool> ApproveLoanAdvanceAsync(long id, long tenantId, long approvedBy);
        Task<IEnumerable<LoanAdvanceDto>> GetLoansAsync(long tenantId);
        Task<IEnumerable<LoanRepaymentDto>> GetLoanRepaymentsAsync(long loanId, long tenantId);

        // Bonus & Incentives
        Task<long> CreateBonusAsync(CreateBonusRequest r);
        Task<IEnumerable<BonusDto>> GetBonusesAsync(long tenantId);

        Task<long> CreateIncentiveAsync(CreateIncentiveRequest r);
        Task<IEnumerable<IncentiveDto>> GetIncentivesAsync(long tenantId);

        // Payslips
        Task<long> GeneratePayslipAsync(long tenantId, long employeeId, long periodId, long createdBy);
        Task<IEnumerable<PayslipDto>> GetPayslipsAsync(long tenantId, long? employeeId, long? periodId);
    }
}
