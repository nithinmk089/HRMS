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
    public class LeaveRepository : ILeaveRepository
    {
        private readonly string _connectionString;
        public LeaveRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateLeaveTypeAsync(CreateLeaveTypeRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@LeaveCode", request.LeaveCode);
            p.Add("@LeaveName", request.LeaveName);
            p.Add("@IsPaid", request.IsPaid);
            p.Add("@IsAccrualBased", request.IsAccrualBased);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@LeaveTypeID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("leave.usp_LeaveType_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@LeaveTypeID");
        }

        public async Task<IEnumerable<LeaveTypeDto>> SearchLeaveTypesAsync(long tenantId, string? searchTerm)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchTerm", searchTerm);
            return await conn.QueryAsync<LeaveTypeDto>("leave.usp_LeaveType_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> CreateLeavePolicyAsync(CreateLeavePolicyRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@PolicyName", request.PolicyName);
            p.Add("@LeaveTypeID", request.LeaveTypeId);
            p.Add("@EffectiveFrom", request.EffectiveFrom);
            p.Add("@EffectiveTo", request.EffectiveTo);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@LeavePolicyID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("leave.usp_LeavePolicy_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@LeavePolicyID");
        }

        public async Task<IEnumerable<LeaveBalanceDto>> GetBalancesAsync(long tenantId, long employeeId, long? leaveTypeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@LeaveTypeID", leaveTypeId);
            return await conn.QueryAsync<LeaveBalanceDto>("leave.usp_LeaveBalance_Get", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> CreateLeaveRequestAsync(CreateLeaveRequestRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@LeaveTypeID", request.LeaveTypeId);
            p.Add("@FromDate", request.FromDate);
            p.Add("@ToDate", request.ToDate);
            p.Add("@TotalDays", request.TotalDays);
            p.Add("@Reason", request.Reason);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@LeaveRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("leave.usp_LeaveRequest_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@LeaveRequestID");
        }

        public async Task<IEnumerable<LeaveRequestDto>> SearchLeaveRequestsAsync(long tenantId, long? employeeId, string? status)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@Status", status);
            return await conn.QueryAsync<LeaveRequestDto>("leave.usp_LeaveRequest_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ApproveLeaveRequestAsync(long id, long tenantId, long approverId, string? remarks)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@LeaveRequestID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            p.Add("@Remarks", remarks);
            var rows = await conn.ExecuteAsync("leave.usp_LeaveRequest_Approve", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> RejectLeaveRequestAsync(long id, long tenantId, long approverId, string? remarks)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@LeaveRequestID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            p.Add("@Remarks", remarks);
            var rows = await conn.ExecuteAsync("leave.usp_LeaveRequest_Reject", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> CancelLeaveRequestAsync(long id, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@LeaveRequestID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var rows = await conn.ExecuteAsync("leave.usp_LeaveRequest_Cancel", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<long> CreateLeaveEncashmentAsync(CreateLeaveEncashmentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@LeaveTypeID", request.LeaveTypeId);
            p.Add("@EncashedDays", request.EncashedDays);
            p.Add("@Amount", request.Amount);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@LeaveEncashmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("leave.usp_LeaveEncashment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@LeaveEncashmentID");
        }

        public async Task<bool> ApproveLeaveEncashmentAsync(long id, long tenantId, long approverId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@LeaveEncashmentID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            var rows = await conn.ExecuteAsync("leave.usp_LeaveEncashment_Approve", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
