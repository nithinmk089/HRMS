using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HRMS.IntegrationTests
{
    public class RolePermissionConfigApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRoleRepository> _roleRepoMock = new();
        private readonly Mock<IPermissionRepository> _permRepoMock = new();
        private readonly Mock<IConfigurationRepository> _configRepoMock = new();

        public RolePermissionConfigApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                    var roleDesc = services.SingleOrDefault(d => d.ServiceType == typeof(IRoleRepository));
                    if (roleDesc != null) services.Remove(roleDesc);
                    services.AddScoped<IRoleRepository>(_ => _roleRepoMock.Object);

                    var permDesc = services.SingleOrDefault(d => d.ServiceType == typeof(IPermissionRepository));
                    if (permDesc != null) services.Remove(permDesc);
                    services.AddScoped<IPermissionRepository>(_ => _permRepoMock.Object);

                    var configDesc = services.SingleOrDefault(d => d.ServiceType == typeof(IConfigurationRepository));
                    if (configDesc != null) services.Remove(configDesc);
                    services.AddScoped<IConfigurationRepository>(_ => _configRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task SearchRoles_ForbiddenForNormalUser()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/roles?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SearchRoles_AllowedForAdmin()
        {
            // Arrange
            _roleRepoMock.Setup(repo => repo.SearchAsync(1, null, 1, 50))
                .ReturnsAsync(new System.Collections.Generic.List<ApplicationRoleDto>());

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.GetAsync("/api/v1/roles?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchPermissions_ForbiddenForNormalUser()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/permissions?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SearchConfigs_ForbiddenForNormalUser()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/configurations?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
