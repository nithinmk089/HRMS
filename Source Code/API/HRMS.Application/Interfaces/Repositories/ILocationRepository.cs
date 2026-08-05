using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface ILocationRepository
    {
        Task<long> CreateAsync(CreateLocationRequest request);
        Task<bool> UpdateAsync(UpdateLocationRequest request);
        Task<bool> DeleteAsync(long locationId, long tenantId, long deletedBy);
        Task<IEnumerable<LocationDto>> SearchAsync(long tenantId, string? searchText, int page, int pageSize);
    }
}
