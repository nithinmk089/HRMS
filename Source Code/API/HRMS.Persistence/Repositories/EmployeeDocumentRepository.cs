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
    public class EmployeeDocumentRepository : IEmployeeDocumentRepository
    {
        private readonly string _connectionString;
        public EmployeeDocumentRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> UploadAsync(UploadEmployeeDocumentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@EmployeeID", request.EmployeeId);
            p.Add("@DocumentType", request.DocumentType);
            p.Add("@FileName", request.FileName);
            p.Add("@FilePath", request.FilePath);
            p.Add("@MimeType", request.MimeType);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeDocumentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeDocument_Upload", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeDocumentID");
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeDocumentRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeDocumentID", request.EmployeeDocumentId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@FileName", request.FileName);
            p.Add("@FilePath", request.FilePath);
            p.Add("@MimeType", request.MimeType);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeDocument_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(long employeeDocumentId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeDocumentID", employeeDocumentId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeDocument_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<EmployeeDocumentDto?> GetByIdAsync(long employeeDocumentId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeDocumentID", employeeDocumentId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<EmployeeDocumentDto>("hr.usp_EmployeeDocument_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmployeeDocumentDto>> SearchAsync(long tenantId, long? employeeId, string? documentType)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@DocumentType", documentType);
            return await conn.QueryAsync<EmployeeDocumentDto>("hr.usp_EmployeeDocument_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> CreateVersionAsync(long employeeDocumentId, long tenantId, string fileName, string filePath, string mimeType, int versionNumber, long createdBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeDocumentID", employeeDocumentId);
            p.Add("@VersionNumber", versionNumber);
            p.Add("@FileName", fileName);
            p.Add("@FilePath", filePath);
            p.Add("@MimeType", mimeType);
            p.Add("@CreatedBy", createdBy);
            p.Add("@EmployeeDocumentVersionID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("hr.usp_EmployeeDocumentVersion_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeDocumentVersionID");
        }

        public async Task<bool> RestoreVersionAsync(long employeeDocumentId, long tenantId, int versionNumber, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeDocumentID", employeeDocumentId);
            p.Add("@TenantID", tenantId);
            p.Add("@VersionNumber", versionNumber);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("hr.usp_EmployeeDocumentVersion_Restore", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }
    }
}