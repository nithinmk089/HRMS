using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected long CurrentUserId
        {
            get
            {
                var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return long.TryParse(claim, out var id) ? id : 1;
            }
        }

        protected long CurrentTenantId
        {
            get
            {
                var claim = User?.FindFirst("tenantId")?.Value;
                return long.TryParse(claim, out var id) ? id : 1;
            }
        }

        protected long? CurrentEmployeeId
        {
            get
            {
                var claim = User?.FindFirst("employeeId")?.Value;
                return long.TryParse(claim, out var id) ? id : null;
            }
        }

        protected bool HasRole(string role) => User?.IsInRole(role) ?? false;

        protected bool IsAdmin => HasRole("SYSADMIN") || 
                                  HasRole("ADMIN") || 
                                  HasRole("HRADMIN");

        protected bool IsSysAdmin => HasRole("SYSADMIN");
    }
}
