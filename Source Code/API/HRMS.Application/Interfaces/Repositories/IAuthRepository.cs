using System.Threading.Tasks;
using HRMS.Application.DTOs.Auth;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<AuthResponse> LoginAsync(string email, string password, string ipAddress, string browserInfo);
        Task<SetupStatusResponse> GetSetupStatusAsync();
        Task<AuthResponse> PerformInitialSetupAsync(InitialSystemSetupRequest request, string ipAddress, string browserInfo);
    }
}
