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
    public class EmployeeCertificationRepository : IEmployeeCertificationRepository
    {
        private readonly string _connectionString;
        public EmployeeCertificationRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateAsync(CreateEmployeeCertificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@CertificationName", request.CertificationName);
            p.Add("@CertificationAuthority", request.CertificationAuthority);
            p.Add("@IssueDate", request.IssueDate);
            p.Add("@ExpiryDate", request.ExpiryDate);
            p.Add("@CertificateNumber", request.CertificateNumber);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeCertificationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeCertification_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeCertificationID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeCertificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeCertificationID", request.EmployeeCertificationId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@CertificationName", request.CertificationName);
            p.Add("@CertificationAuthority", request.CertificationAuthority);
            p.Add("@IssueDate", request.IssueDate);
            p.Add("@ExpiryDate", request.ExpiryDate);
            p.Add("@CertificateNumber", request.CertificateNumber);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeCertification_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeCertificationId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeCertificationID", employeeCertificationId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeCertification_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<EmployeeCertificationDto>> GetByEmployeeIdAsync(long employeeId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<EmployeeCertificationDto>(
                "SELECT * FROM hr.EmployeeCertification WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0",
                new { EmployeeID = employeeId, TenantID = tenantId }
            );
        }

        public async Task<IEnumerable<CertificationExpiryReportDto>> GetCertificationExpiryReportAsync(long tenantId, int withinDays)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@WithinDays", withinDays);
            return await conn.QueryAsync<CertificationExpiryReportDto>("hr.usp_Report_CertificationExpiry", p, commandType: CommandType.StoredProcedure);
        }
    }
}