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
    public class ShiftRepository : IShiftRepository
    {
        private readonly string _connectionString;
        public ShiftRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateShiftAsync(CreateShiftRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@ShiftCode", request.ShiftCode);
            p.Add("@ShiftName", request.ShiftName);
            p.Add("@ShiftType", request.ShiftType);
            p.Add("@StartTime", request.StartTime);
            p.Add("@EndTime", request.EndTime);
            p.Add("@GraceInMinutes", request.GraceInMinutes);
            p.Add("@GraceOutMinutes", request.GraceOutMinutes);
            p.Add("@IsFlexible", request.IsFlexible);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ShiftID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("attendance.usp_Shift_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ShiftID");
        }

        public async Task<bool> UpdateShiftAsync(UpdateShiftRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ShiftID", request.ShiftId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@ShiftCode", request.ShiftCode);
            p.Add("@ShiftName", request.ShiftName);
            p.Add("@ShiftType", request.ShiftType);
            p.Add("@StartTime", request.StartTime);
            p.Add("@EndTime", request.EndTime);
            p.Add("@GraceInMinutes", request.GraceInMinutes);
            p.Add("@GraceOutMinutes", request.GraceOutMinutes);
            p.Add("@IsFlexible", request.IsFlexible);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var rows = await conn.ExecuteAsync("attendance.usp_Shift_Update", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> DeleteShiftAsync(long shiftId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ShiftID", shiftId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var rows = await conn.ExecuteAsync("attendance.usp_Shift_Delete", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<ShiftDto?> GetShiftByIdAsync(long shiftId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ShiftID", shiftId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<ShiftDto>("attendance.usp_Shift_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ShiftDto>> SearchShiftsAsync(long tenantId, string? searchTerm)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchTerm", searchTerm);
            return await conn.QueryAsync<ShiftDto>("attendance.usp_Shift_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> CreateShiftAssignmentAsync(CreateShiftAssignmentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@ShiftID", request.ShiftId);
            p.Add("@EffectiveFrom", request.EffectiveFrom);
            p.Add("@EffectiveTo", request.EffectiveTo);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ShiftAssignmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("attendance.usp_ShiftAssignment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ShiftAssignmentID");
        }

        public async Task<bool> UpdateShiftAssignmentAsync(long id, long tenantId, long shiftId, DateTime fromDate, DateTime? toDate, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ShiftAssignmentID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ShiftID", shiftId);
            p.Add("@EffectiveFrom", fromDate);
            p.Add("@EffectiveTo", toDate);
            p.Add("@ModifiedBy", modifiedBy);
            var rows = await conn.ExecuteAsync("attendance.usp_ShiftAssignment_Update", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> DeleteShiftAssignmentAsync(long id, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@ShiftAssignmentID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var rows = await conn.ExecuteAsync("attendance.usp_ShiftAssignment_Delete", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
