using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/payroll")]
    [ApiController]
    [Authorize]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollRepository _payrollRepository;
        public PayrollController(IPayrollRepository payrollRepository) => _payrollRepository = payrollRepository;

        // --- Calendars ---
        [HttpGet("calendars")]
        public async Task<IActionResult> SearchCalendars([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var res = await _payrollRepository.SearchCalendarsAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<IEnumerable<PayrollCalendarDto>>.SuccessResult(res));
        }

        [HttpPost("calendars")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateCalendar([FromBody] CreatePayrollCalendarRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateCalendarAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Payroll calendar created."));
        }

        [HttpPut("calendars/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateCalendar(long id, [FromBody] UpdatePayrollCalendarRequest r)
        {
            r.PayrollCalendarID = id;
            r.ModifiedBy = 1;
            var ok = await _payrollRepository.UpdateCalendarAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Payroll calendar updated."));
        }

        [HttpDelete("calendars/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> DeleteCalendar(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.DeleteCalendarAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Payroll calendar deleted."));
        }

        // --- Periods ---
        [HttpGet("periods")]
        public async Task<IActionResult> GetPeriods([FromQuery] long tenantId, [FromQuery] long calendarId)
        {
            var res = await _payrollRepository.GetPeriodsAsync(tenantId, calendarId);
            return Ok(ApiResponse<IEnumerable<PayrollPeriodDto>>.SuccessResult(res));
        }

        [HttpPost("periods")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreatePeriod([FromBody] CreatePayrollPeriodRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreatePeriodAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Payroll period created."));
        }

        [HttpPost("periods/{id}/open")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> OpenPeriod(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.OpenPeriodAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Period status updated to Open."));
        }

        [HttpPost("periods/{id}/close")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ClosePeriod(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.ClosePeriodAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Period status updated to Closed."));
        }

        [HttpPost("periods/{id}/lock")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> LockPeriod(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.LockPeriodAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Period locked."));
        }

        // --- Salary Structures ---
        [HttpGet("salary-structures")]
        public async Task<IActionResult> GetSalaryStructures([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetSalaryStructuresAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<SalaryStructureDto>>.SuccessResult(res));
        }

        [HttpPost("salary-structures")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateSalaryStructure([FromBody] CreateSalaryStructureRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateSalaryStructureAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Salary structure template added."));
        }

        [HttpPut("salary-structures/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateSalaryStructure(long id, [FromBody] UpdateSalaryStructureRequest r)
        {
            r.SalaryStructureID = id;
            r.ModifiedBy = 1;
            var ok = await _payrollRepository.UpdateSalaryStructureAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Salary structure template updated."));
        }

        // --- Salary Components ---
        [HttpGet("salary-components")]
        public async Task<IActionResult> GetSalaryComponents([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetSalaryComponentsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<SalaryComponentDto>>.SuccessResult(res));
        }

        [HttpPost("salary-components")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateSalaryComponent([FromBody] CreateSalaryComponentRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateSalaryComponentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Salary component template created."));
        }

        // --- Employee Compensation ---
        [HttpGet("compensations")]
        public async Task<IActionResult> GetEmployeeCompensations([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetEmployeeCompensationsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeCompensationDto>>.SuccessResult(res));
        }

        [HttpPost("compensations")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HR")]
        public async Task<IActionResult> CreateEmployeeCompensation([FromBody] CreateEmployeeCompensationRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateEmployeeCompensationAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Employee compensation framework mapped."));
        }

        // --- Payroll Runs ---
        [HttpGet("runs")]
        public async Task<IActionResult> GetPayrollRuns([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetPayrollRunsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<PayrollRunDto>>.SuccessResult(res));
        }

        [HttpPost("runs")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreatePayrollRun([FromBody] CreatePayrollRunRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreatePayrollRunAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Payroll processing run initiated."));
        }

        [HttpPost("runs/{id}/process")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ProcessRun(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.ProcessPayrollRunAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Payroll engine run calculation completed."));
        }

        [HttpGet("runs/{id}/transactions")]
        public async Task<IActionResult> GetTransactions(long id, [FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetPayrollTransactionsAsync(id, tenantId);
            return Ok(ApiResponse<IEnumerable<PayrollTransactionDto>>.SuccessResult(res));
        }

        // --- Adjustments ---
        [HttpGet("adjustments")]
        public async Task<IActionResult> GetAdjustments([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetAdjustmentsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<PayrollAdjustmentDto>>.SuccessResult(res));
        }

        [HttpPost("adjustments")]
        public async Task<IActionResult> CreateAdjustment([FromBody] CreatePayrollAdjustmentRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateAdjustmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Adjustment record created."));
        }

        [HttpPost("adjustments/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ApproveAdjustment(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.ApproveAdjustmentAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Adjustment verified."));
        }

        // --- Loans ---
        [HttpGet("loans")]
        public async Task<IActionResult> GetLoans([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetLoansAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<LoanAdvanceDto>>.SuccessResult(res));
        }

        [HttpPost("loans")]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateLoanAdvanceAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Loan advance requested."));
        }

        [HttpPost("loans/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ApproveLoan(long id, [FromQuery] long tenantId)
        {
            var ok = await _payrollRepository.ApproveLoanAdvanceAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Loan advance approved and repayments generated."));
        }

        [HttpGet("loans/{id}/repayments")]
        public async Task<IActionResult> GetRepayments(long id, [FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetLoanRepaymentsAsync(id, tenantId);
            return Ok(ApiResponse<IEnumerable<LoanRepaymentDto>>.SuccessResult(res));
        }

        // --- Bonus & Incentives ---
        [HttpGet("bonuses")]
        public async Task<IActionResult> GetBonuses([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetBonusesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<BonusDto>>.SuccessResult(res));
        }

        [HttpPost("bonuses")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateBonus([FromBody] CreateBonusRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateBonusAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Bonus record created."));
        }

        [HttpGet("incentives")]
        public async Task<IActionResult> GetIncentives([FromQuery] long tenantId)
        {
            var res = await _payrollRepository.GetIncentivesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<IncentiveDto>>.SuccessResult(res));
        }

        [HttpPost("incentives")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateIncentive([FromBody] CreateIncentiveRequest r)
        {
            r.CreatedBy = 1;
            var id = await _payrollRepository.CreateIncentiveAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Incentive record logged."));
        }

        // --- Payslips ---
        [HttpGet("payslips")]
        public async Task<IActionResult> GetPayslips([FromQuery] long tenantId, [FromQuery] long? employeeId, [FromQuery] long? periodId)
        {
            var res = await _payrollRepository.GetPayslipsAsync(tenantId, employeeId, periodId);
            return Ok(ApiResponse<IEnumerable<PayslipDto>>.SuccessResult(res));
        }

        [HttpPost("payslips/generate")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> GeneratePayslip([FromQuery] long tenantId, [FromQuery] long employeeId, [FromQuery] long periodId)
        {
            var id = await _payrollRepository.GeneratePayslipAsync(tenantId, employeeId, periodId, 1);
            return Ok(ApiResponse<long>.SuccessResult(id, "Payslip generated successfully."));
        }
    }
}
