using System.IO;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Services
{
    public interface IDataImportService
    {
        byte[] GenerateTemplateCsv(string entityType);
        Task<ImportResultDto> ImportEmployeesAsync(long tenantId, long userId, Stream fileStream, string fileName);
        Task<ImportResultDto> ImportMasterDataAsync(long tenantId, long userId, string entityType, Stream fileStream, string fileName);
        Task<ImportResultDto> ImportOperationalDataAsync(long tenantId, long userId, string entityType, Stream fileStream, string fileName);
    }
}
