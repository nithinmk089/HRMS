using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class EmployeeEmploymentRepository : IEmployeeEmploymentRepository
    {
        private readonly string _connectionString;
        public EmployeeEmploymentRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeEmploymentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@CompanyID", request.CompanyId);
            p.Add("@BusinessUnitID", request.BusinessUnitId);
            p.Add("@DepartmentID", request.DepartmentId);
            p.Add("@DesignationID", request.DesignationId);
            p.Add("@LocationID", request.LocationId);
            p.Add("@CostCenterID", request.CostCenterId);
            p.Add("@EmploymentType", request.EmploymentType);
            p.Add("@JoiningDate", request.JoiningDate);
            p.Add("@ConfirmationDate", request.ConfirmationDate);
            p.Add("@ProbationEndDate", request.ProbationEndDate);
            p.Add("@NoticePeriodDays", request.NoticePeriodDays);
            p.Add("@EmploymentStatus", request.EmploymentStatus);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeEmploymentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeEmployment_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeEmploymentID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeEmploymentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeEmploymentID", request.EmployeeEmploymentId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@CompanyID", request.CompanyId);
            p.Add("@BusinessUnitID", request.BusinessUnitId);
            p.Add("@DepartmentID", request.DepartmentId);
            p.Add("@DesignationID", request.DesignationId);
            p.Add("@LocationID", request.LocationId);
            p.Add("@CostCenterID", request.CostCenterId);
            p.Add("@EmploymentType", request.EmploymentType);
            p.Add("@JoiningDate", request.JoiningDate);
            p.Add("@ConfirmationDate", request.ConfirmationDate);
            p.Add("@ProbationEndDate", request.ProbationEndDate);
            p.Add("@NoticePeriodDays", request.NoticePeriodDays);
            p.Add("@EmploymentStatus", request.EmploymentStatus);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeEmployment_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<EmployeeEmploymentDto?> GetByEmployeeIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeID", employeeId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<EmployeeEmploymentDto>("hr.usp_EmployeeEmployment_Search", p, commandType: CommandType.StoredProcedure);
        }
    }
}