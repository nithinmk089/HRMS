using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HRMS.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new System.Collections.Generic.List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("tenantId", "1"),
                new Claim("employeeId", "1")
            };

            if (Request.Headers.TryGetValue("X-Test-Role", out var roles) && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "SYSADMIN"));
            }

            if (Request.Headers.TryGetValue("X-Test-TenantId", out var tid) && tid.Count > 0)
            {
                claims.RemoveAll(c => c.Type == "tenantId");
                claims.Add(new Claim("tenantId", tid[0]!));
            }

            if (Request.Headers.TryGetValue("X-Test-UserId", out var uid) && uid.Count > 0)
            {
                claims.RemoveAll(c => c.Type == ClaimTypes.NameIdentifier);
                claims.Add(new Claim(ClaimTypes.NameIdentifier, uid[0]!));
            }

            if (Request.Headers.TryGetValue("X-Test-EmployeeId", out var eid) && eid.Count > 0)
            {
                claims.RemoveAll(c => c.Type == "employeeId");
                claims.Add(new Claim("employeeId", eid[0]!));
            }

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
