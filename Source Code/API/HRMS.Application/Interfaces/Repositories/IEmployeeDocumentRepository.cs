using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IEmployeeDocumentRepository
    {
        Task<long> UploadAsync(UploadEmployeeDocumentRequest request);
        Task<bool> UpdateAsync(UpdateEmployeeDocumentRequest request);
        Task<bool> DeleteAsync(long employeeDocumentId, long tenantId, long deletedBy);
        Task<EmployeeDocumentDto?> GetByIdAsync(long employeeDocumentId, long tenantId);
        Task<IEnumerable<EmployeeDocumentDto>> SearchAsync(long tenantId, long? employeeId, string? documentType);
        Task<long> CreateVersionAsync(long employeeDocumentId, long tenantId, string fileName, string filePath, string mimeType, int versionNumber, long createdBy);
        Task<bool> RestoreVersionAsync(long employeeDocumentId, long tenantId, int versionNumber, long modifiedBy);
    }
}