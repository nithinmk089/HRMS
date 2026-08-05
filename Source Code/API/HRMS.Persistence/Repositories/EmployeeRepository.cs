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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        public EmployeeRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeCode", request.EmployeeCode);
            p.Add("@EmployeeNumber", request.EmployeeNumber);
            p.Add("@FirstName", request.FirstName);
            p.Add("@MiddleName", request.MiddleName);
            p.Add("@LastName", request.LastName);
            p.Add("@PreferredName", request.PreferredName);
            p.Add("@Gender", request.Gender);
            p.Add("@DateOfBirth", request.DateOfBirth);
            p.Add("@MaritalStatus", request.MaritalStatus);
            p.Add("@Nationality", request.Nationality);
            p.Add("@PersonalEmail", request.PersonalEmail);
            p.Add("@MobileNumber", request.MobileNumber);
            p.Add("@Status", request.Status);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_Employee_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@FirstName", request.FirstName);
            p.Add("@MiddleName", request.MiddleName);
            p.Add("@LastName", request.LastName);
            p.Add("@PreferredName", request.PreferredName);
            p.Add("@Gender", request.Gender);
            p.Add("@DateOfBirth", request.DateOfBirth);
            p.Add("@MaritalStatus", request.MaritalStatus);
            p.Add("@Nationality", request.Nationality);
            p.Add("@PersonalEmail", request.PersonalEmail);
            p.Add("@MobileNumber", request.MobileNumber);
            p.Add("@Status", request.Status);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<EmployeeDto?> GetByIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<EmployeeDto>("hr.usp_Employee_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmployeeDirectoryDto>> SearchAsync(long tenantId, string? searchText, string? status, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@Status", status);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("hr.usp_Employee_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count row
            return await multi.ReadAsync<EmployeeDirectoryDto>();
        }

        public async Task<bool> ActivateAsync(long employeeId, long tenantId, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_Activate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> SuspendAsync(long employeeId, long tenantId, string? reason, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@Reason", reason);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_Suspend", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> TerminateAsync(long employeeId, long tenantId, string? reason, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@Reason", reason);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_Terminate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> RehireAsync(long employeeId, long tenantId, string? reason, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@Reason", reason);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_Rehire", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> UpdateContactDetailsAsync(long employeeId, long tenantId, string? personalEmail, string? mobileNumber, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@PersonalEmail", personalEmail);
            p.Add("@MobileNumber", mobileNumber);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_UpdateContactDetails", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> UpdateProfileAsync(long employeeId, long tenantId, string? preferredName, string? maritalStatus, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            p.Add("@PreferredName", preferredName);
            p.Add("@MaritalStatus", maritalStatus);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_Employee_UpdateProfile", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeServiceHistoryReportDto>> GetServiceHistoryReportAsync(long tenantId, long employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            return await conn.QueryAsync<EmployeeServiceHistoryReportDto>("hr.usp_Report_EmployeeServiceHistory", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmployeeDirectoryDto>> GetDirectoryReportAsync(long tenantId, long? departmentId, long? locationId, string? status)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@DepartmentID", departmentId);
            p.Add("@LocationID", locationId);
            p.Add("@Status", status);
            return await conn.QueryAsync<EmployeeDirectoryDto>("hr.usp_Report_EmployeeDirectory", p, commandType: CommandType.StoredProcedure);
        }
    }
}