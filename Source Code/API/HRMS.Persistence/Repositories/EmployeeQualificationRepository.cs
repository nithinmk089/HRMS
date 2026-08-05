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
    public class EmployeeQualificationRepository : IEmployeeQualificationRepository
    {
        private readonly string _connectionString;
        public EmployeeQualificationRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeQualificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@QualificationType", request.QualificationType);
            p.Add("@Institution", request.Institution);
            p.Add("@University", request.University);
            p.Add("@YearOfPassing", request.YearOfPassing);
            p.Add("@Percentage", request.Percentage);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeQualificationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeQualification_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeQualificationID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeQualificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeQualificationID", request.EmployeeQualificationId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@QualificationType", request.QualificationType);
            p.Add("@Institution", request.Institution);
            p.Add("@University", request.University);
            p.Add("@YearOfPassing", request.YearOfPassing);
            p.Add("@Percentage", request.Percentage);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeQualification_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeQualificationId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeQualificationID", employeeQualificationId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeQualification_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeQualificationDto>> GetByEmployeeIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeQualificationDto>(
                "SELECT * FROM hr.EmployeeQualification WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0",
                new { EmployeeID = employeeId, TenantID = tenantId }
            );
        }
    }
}