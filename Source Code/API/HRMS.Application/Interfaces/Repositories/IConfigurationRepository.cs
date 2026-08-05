using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IConfigurationRepository
    {
        Task<long> CreateAsync(CreateConfigurationRequest request);
        Task<bool> UpdateAsync(UpdateConfigurationRequest request);
        Task<bool> DeleteAsync(long configurationId, long tenantId, long deletedBy);
        Task<IEnumerable<ConfigurationDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
        Task<string?> GetValueAsync(long tenantId, string key);
        Task<bool> SetValueAsync(long tenantId, string key, string value, string dataType = "String", long modifiedBy = 1);
    }
}
