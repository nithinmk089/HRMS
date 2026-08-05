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
    public class EmployeeTransferRepository : IEmployeeTransferRepository
    {
        private readonly string _connectionString;
        public EmployeeTransferRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeTransferRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@FromDepartmentID", request.FromDepartmentId);
            p.Add("@ToDepartmentID", request.ToDepartmentId);
            p.Add("@FromLocationID", request.FromLocationId);
            p.Add("@ToLocationID", request.ToLocationId);
            p.Add("@EffectiveDate", request.EffectiveDate);
            p.Add("@Reason", request.Reason);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeTransferID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeTransfer_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeTransferID");
        }

        public async Task<bool> ApproveAsync(long employeeTransferId, long tenantId, long approvedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeTransferID", employeeTransferId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", approvedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeTransfer_Approve", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CompleteAsync(long employeeTransferId, long tenantId, long completedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeTransferID", employeeTransferId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", completedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeTransfer_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeTransferDto>> GetTransferHistoryReportAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            return await conn.QueryAsync<EmployeeTransferDto>("hr.usp_Report_EmployeeTransfers", p, commandType: CommandType.StoredProcedure);
        }
    }
}