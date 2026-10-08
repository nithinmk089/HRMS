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
                if (long.TryParse(claim, out var id)) return id;
                if (User?.Identity?.IsAuthenticated == true)
                {
                    throw new UnauthorizedAccessException("User identity claim could not be determined.");
                }
                return 1;
            }
        }

        protected long CurrentTenantId
        {
            get
            {
                var claim = User?.FindFirst("tenantId")?.Value;
                if (long.TryParse(claim, out var id)) return id;
                if (User?.Identity?.IsAuthenticated == true)
                {
                    throw new UnauthorizedAccessException("Tenant context claim could not be determined.");
                }
                return 1;
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

        /// <summary>
        /// Enforces multi-tenant isolation.
        /// Only SYSADMIN users can access another tenant's data by explicitly supplying requestedTenantId.
        /// Non-SYSADMIN users requesting a different tenant ID are rejected with UnauthorizedAccessException.
        /// </summary>
        protected long GetEffectiveTenantId(long? requestedTenantId = null)
        {
            var userTenantId = CurrentTenantId;
            if (requestedTenantId.HasValue && requestedTenantId.Value > 0)
            {
                if (IsSysAdmin)
                {
                    return requestedTenantId.Value;
                }

                if (requestedTenantId.Value != userTenantId && User?.Identity?.IsAuthenticated == true)
                {
                    throw new UnauthorizedAccessException($"Cross-tenant access forbidden. You cannot access data for Tenant {requestedTenantId.Value}.");
                }
                return userTenantId;
            }
            return userTenantId;
        }
    }
}
