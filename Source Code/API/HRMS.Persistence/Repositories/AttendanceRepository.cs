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
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly string _connectionString;
        public AttendanceRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> ClockInAsync(CreateAttendanceRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@ShiftID", request.ShiftId);
            p.Add("@AttendanceDate", request.AttendanceDate);
            p.Add("@ClockInTime", request.ClockInTime);
            p.Add("@AttendanceStatus", request.AttendanceStatus);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@AttendanceID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("attendance.usp_Attendance_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AttendanceID");
        }

        public async Task<bool> ClockOutAsync(UpdateAttendanceRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AttendanceID", request.AttendanceId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@ClockOutTime", request.ClockOutTime);
            p.Add("@WorkingMinutes", request.WorkingMinutes);
            p.Add("@AttendanceStatus", request.AttendanceStatus);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var rows = await conn.ExecuteAsync("attendance.usp_Attendance_Update", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<AttendanceDto?> GetByIdAsync(long attendanceId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AttendanceID", attendanceId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<AttendanceDto>("attendance.usp_Attendance_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AttendanceDto>> SearchAsync(long tenantId, long? employeeId, DateTime? startDate, DateTime? endDate)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@StartDate", startDate);
            p.Add("@EndDate", endDate);
            return await conn.QueryAsync<AttendanceDto>("attendance.usp_Attendance_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> RecalculateAsync(long tenantId, long employeeId, DateTime attendanceDate, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@AttendanceDate", attendanceDate);
            p.Add("@ModifiedBy", modifiedBy);
            var rows = await conn.ExecuteAsync("attendance.usp_Attendance_Recalculate", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<long> CreateAdjustmentAsync(CreateAttendanceAdjustmentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@AttendanceID", request.AttendanceId);
            p.Add("@AdjustmentReason", request.AdjustmentReason);
            p.Add("@OriginalValue", request.OriginalValue);
            p.Add("@NewValue", request.NewValue);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@AttendanceAdjustmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("attendance.usp_AttendanceAdjustment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AttendanceAdjustmentID");
        }

        public async Task<bool> ApproveAdjustmentAsync(long id, long tenantId, long approverId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AttendanceAdjustmentID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            var rows = await conn.ExecuteAsync("attendance.usp_AttendanceAdjustment_Approve", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> RejectAdjustmentAsync(long id, long tenantId, long approverId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AttendanceAdjustmentID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            var rows = await conn.ExecuteAsync("attendance.usp_AttendanceAdjustment_Reject", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<long> CreateRegularizationAsync(CreateAttendanceRegularizationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@RequestedDate", request.RequestedDate);
            p.Add("@Reason", request.Reason);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@AttendanceRegularizationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("attendance.usp_AttendanceRegularization_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@AttendanceRegularizationID");
        }

        public async Task<bool> ApproveRegularizationAsync(long id, long tenantId, long approverId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AttendanceRegularizationID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            var rows = await conn.ExecuteAsync("attendance.usp_AttendanceRegularization_Approve", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> RejectRegularizationAsync(long id, long tenantId, long approverId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@AttendanceRegularizationID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            var rows = await conn.ExecuteAsync("attendance.usp_AttendanceRegularization_Reject", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
