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
    public class OvertimeRepository : IOvertimeRepository
    {
        private readonly string _connectionString;
        public OvertimeRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateOvertimeRequestAsync(CreateOvertimeRequestRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@OvertimeDate", request.OvertimeDate);
            p.Add("@RequestedHours", request.RequestedHours);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@OvertimeRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("attendance.usp_OvertimeRequest_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@OvertimeRequestID");
        }

        public async Task<IEnumerable<OvertimeRequestDto>> SearchOvertimeRequestsAsync(long tenantId, long? employeeId, string? status)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@Status", status);
            return await conn.QueryAsync<OvertimeRequestDto>("attendance.usp_OvertimeRequest_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ApproveOvertimeRequestAsync(long id, long tenantId, long approverId, string? remarks)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OvertimeRequestID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            p.Add("@Remarks", remarks);
            var rows = await conn.ExecuteAsync("attendance.usp_OvertimeRequest_Approve", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> RejectOvertimeRequestAsync(long id, long tenantId, long approverId, string? remarks)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OvertimeRequestID", id);
            p.Add("@TenantID", tenantId);
            p.Add("@ApproverID", approverId);
            p.Add("@Remarks", remarks);
            var rows = await conn.ExecuteAsync("attendance.usp_OvertimeRequest_Reject", p, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
