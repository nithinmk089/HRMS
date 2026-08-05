using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly string _connectionString;
        public PayrollRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Calendars
        public async Task<long> CreateCalendarAsync(CreatePayrollCalendarRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@CalendarCode", r.CalendarCode);
            p.Add("@CalendarName", r.CalendarName);
            p.Add("@FinancialYear", r.FinancialYear);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@PayrollCalendarID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_PayrollCalendar_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PayrollCalendarID");
        }

        public async Task<bool> UpdateCalendarAsync(UpdatePayrollCalendarRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@PayrollCalendarID", r.PayrollCalendarID);
            p.Add("@TenantID", r.TenantID);
            p.Add("@CalendarName", r.CalendarName);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("payroll.usp_PayrollCalendar_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteCalendarAsync(long id, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@PayrollCalendarID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("payroll.usp_PayrollCalendar_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<PayrollCalendarDto>> SearchCalendarsAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("payroll.usp_PayrollCalendar_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<PayrollCalendarDto>();
        }

        // Periods
        public async Task<long> CreatePeriodAsync(CreatePayrollPeriodRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@PayrollCalendarID", r.PayrollCalendarID);
            p.Add("@PeriodCode", r.PeriodCode);
            p.Add("@PeriodStartDate", r.PeriodStartDate);
            p.Add("@PeriodEndDate", r.PeriodEndDate);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@PayrollPeriodID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_PayrollPeriod_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PayrollPeriodID");
        }

        public async Task<bool> OpenPeriodAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("payroll.usp_PayrollPeriod_Open", new { PayrollPeriodID = id, TenantID = tenantId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> ClosePeriodAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("payroll.usp_PayrollPeriod_Close", new { PayrollPeriodID = id, TenantID = tenantId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> LockPeriodAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("payroll.usp_PayrollPeriod_Lock", new { PayrollPeriodID = id, TenantID = tenantId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<PayrollPeriodDto>> GetPeriodsAsync(long tenantId, long calendarId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<PayrollPeriodDto>("SELECT * FROM payroll.PayrollPeriod WHERE TenantID = @TenantID AND PayrollCalendarID = @CalendarID AND IsDeleted = 0", new { TenantID = tenantId, CalendarID = calendarId });
        }

        // Salary Structures
        public async Task<long> CreateSalaryStructureAsync(CreateSalaryStructureRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@StructureCode", r.StructureCode);
            p.Add("@StructureName", r.StructureName);
            p.Add("@EffectiveFrom", r.EffectiveFrom);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@SalaryStructureID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_SalaryStructure_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@SalaryStructureID");
        }

        public async Task<bool> UpdateSalaryStructureAsync(UpdateSalaryStructureRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@SalaryStructureID", r.SalaryStructureID);
            p.Add("@TenantID", r.TenantID);
            p.Add("@StructureName", r.StructureName);
            p.Add("@EffectiveTo", r.EffectiveTo);
            p.Add("@ModifiedBy", r.ModifiedBy);
            var affected = await conn.ExecuteAsync("payroll.usp_SalaryStructure_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<SalaryStructureDto>> GetSalaryStructuresAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<SalaryStructureDto>("SELECT * FROM payroll.SalaryStructure WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Salary Components
        public async Task<long> CreateSalaryComponentAsync(CreateSalaryComponentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@ComponentCode", r.ComponentCode);
            p.Add("@ComponentName", r.ComponentName);
            p.Add("@ComponentType", r.ComponentType);
            p.Add("@CalculationMethod", r.CalculationMethod);
            p.Add("@TaxableFlag", r.TaxableFlag);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@SalaryComponentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_SalaryComponent_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@SalaryComponentID");
        }

        public async Task<IEnumerable<SalaryComponentDto>> GetSalaryComponentsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<SalaryComponentDto>("SELECT * FROM payroll.SalaryComponent WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Employee Compensation
        public async Task<long> CreateEmployeeCompensationAsync(CreateEmployeeCompensationRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@SalaryStructureID", r.SalaryStructureID);
            p.Add("@GrossSalary", r.GrossSalary);
            p.Add("@EffectiveFrom", r.EffectiveFrom);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@EmployeeCompensationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_EmployeeCompensation_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeCompensationID");
        }

        public async Task<IEnumerable<EmployeeCompensationDto>> GetEmployeeCompensationsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeCompensationDto>("SELECT * FROM payroll.vw_Payslip WHERE TenantID = @TenantID", new { TenantID = tenantId }); // Fallback select
        }

        // Payroll Run
        public async Task<long> CreatePayrollRunAsync(CreatePayrollRunRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@PayrollPeriodID", r.PayrollPeriodID);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@PayrollRunID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_PayrollRun_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PayrollRunID");
        }

        public async Task<bool> ProcessPayrollRunAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("payroll.usp_PayrollRun_Process", new { PayrollRunID = id, TenantID = tenantId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<PayrollRunDto>> GetPayrollRunsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<PayrollRunDto>("SELECT * FROM payroll.vw_PayrollSummary WHERE TenantID = @TenantID", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<PayrollTransactionDto>> GetPayrollTransactionsAsync(long runId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<PayrollTransactionDto>("SELECT * FROM payroll.vw_SalaryRegister WHERE PayrollPeriodID = (SELECT PayrollPeriodID FROM payroll.PayrollRun WHERE PayrollRunID = @RunID) AND TenantID = @TenantID", new { RunID = runId, TenantID = tenantId });
        }

        // Adjustments
        public async Task<long> CreateAdjustmentAsync(CreatePayrollAdjustmentRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@PayrollPeriodID", r.PayrollPeriodID);
            p.Add("@AdjustmentType", r.AdjustmentType);
            p.Add("@AdjustmentAmount", r.AdjustmentAmount);
            p.Add("@Remarks", r.Remarks);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@PayrollAdjustmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("INSERT INTO payroll.PayrollAdjustment (TenantID, EmployeeID, PayrollPeriodID, AdjustmentType, AdjustmentAmount, Remarks, AdjustmentStatus, CreatedBy) VALUES (@TenantID, @EmployeeID, @PayrollPeriodID, @AdjustmentType, @AdjustmentAmount, @Remarks, 'Pending', @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", p);
            return p.Get<long>("@PayrollAdjustmentID");
        }

        public async Task<bool> ApproveAdjustmentAsync(long id, long tenantId, long approvedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("UPDATE payroll.PayrollAdjustment SET AdjustmentStatus = 'Approved', ModifiedBy = @ApprovedBy, ModifiedDate = GETUTCDATE() WHERE PayrollAdjustmentID = @ID AND TenantID = @TenantID", new { ID = id, TenantID = tenantId, ApprovedBy = approvedBy });
            return affected > 0;
        }

        public async Task<IEnumerable<PayrollAdjustmentDto>> GetAdjustmentsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<PayrollAdjustmentDto>("SELECT * FROM payroll.PayrollAdjustment WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Loans & Advances
        public async Task<long> CreateLoanAdvanceAsync(CreateLoanRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", r.TenantID);
            p.Add("@EmployeeID", r.EmployeeID);
            p.Add("@LoanType", r.LoanType);
            p.Add("@PrincipalAmount", r.PrincipalAmount);
            p.Add("@InterestRate", r.InterestRate);
            p.Add("@TenureMonths", r.TenureMonths);
            p.Add("@CreatedBy", r.CreatedBy);
            p.Add("@LoanAdvanceID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_LoanAdvance_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@LoanAdvanceID");
        }

        public async Task<bool> ApproveLoanAdvanceAsync(long id, long tenantId, long approvedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var affected = await conn.ExecuteAsync("payroll.usp_LoanAdvance_Approve", new { LoanAdvanceID = id, TenantID = tenantId, ApprovedBy = approvedBy }, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<LoanAdvanceDto>> GetLoansAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<LoanAdvanceDto>("SELECT * FROM payroll.LoanAdvance WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<IEnumerable<LoanRepaymentDto>> GetLoanRepaymentsAsync(long loanId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<LoanRepaymentDto>("SELECT * FROM payroll.LoanRepayment WHERE LoanAdvanceID = @LoanID AND TenantID = @TenantID AND IsDeleted = 0", new { LoanID = loanId, TenantID = tenantId });
        }

        // Bonus & Incentives
        public async Task<long> CreateBonusAsync(CreateBonusRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO payroll.Bonus (TenantID, EmployeeID, BonusType, BonusAmount, BonusPeriod, Status, CreatedBy) VALUES (@TenantID, @EmployeeID, @BonusType, @BonusAmount, @BonusPeriod, 'Pending', @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<BonusDto>> GetBonusesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<BonusDto>("SELECT * FROM payroll.Bonus WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        public async Task<long> CreateIncentiveAsync(CreateIncentiveRequest r)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<long>("INSERT INTO payroll.Incentive (TenantID, EmployeeID, IncentiveType, IncentiveAmount, IncentivePeriod, Status, CreatedBy) VALUES (@TenantID, @EmployeeID, @IncentiveType, @IncentiveAmount, @IncentivePeriod, 'Pending', @CreatedBy); SELECT CAST(SCOPE_IDENTITY() as bigint);", r);
        }

        public async Task<IEnumerable<IncentiveDto>> GetIncentivesAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<IncentiveDto>("SELECT * FROM payroll.Incentive WHERE TenantID = @TenantID AND IsDeleted = 0", new { TenantID = tenantId });
        }

        // Payslips
        public async Task<long> GeneratePayslipAsync(long tenantId, long employeeId, long periodId, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@PayrollPeriodID", periodId);
            p.Add("@CreatedBy", createdBy);
            p.Add("@PayslipID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("payroll.usp_Payslip_Generate", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PayslipID");
        }

        public async Task<IEnumerable<PayslipDto>> GetPayslipsAsync(long tenantId, long? employeeId, long? periodId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM payroll.vw_Payslip WHERE TenantID = @TenantID";
            if (employeeId.HasValue) sql += " AND EmployeeID = @EmployeeID";
            if (periodId.HasValue) sql += " AND PayrollPeriodID = @PeriodID";
            return await conn.QueryAsync<PayslipDto>(sql, new { TenantID = tenantId, EmployeeID = employeeId, PeriodID = periodId });
        }
    }
}
