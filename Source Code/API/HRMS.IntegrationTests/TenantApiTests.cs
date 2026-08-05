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
    public class TenantApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<ITenantRepository> _tenantRepoMock = new();

        public TenantApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITenantRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<ITenantRepository>(_ => _tenantRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetTenantById_ReturnsSuccessWrapper_WhenExists()
        {
            // Arrange
            var tenantDto = new TenantDto { TenantId = 1L, TenantName = "Integration Tenant", Status = "Active" };
            _tenantRepoMock.Setup(repo => repo.GetByIdAsync(1L)).ReturnsAsync(tenantDto);
            
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/tenants/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TenantDto>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Integration Tenant", result.Data.TenantName);
        }

        [Fact]
        public async Task GetTenantById_ReturnsFailureWrapper_WhenNotFound()
        {
            // Arrange
            _tenantRepoMock.Setup(repo => repo.GetByIdAsync(999L)).ReturnsAsync((TenantDto)null);
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/tenants/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TenantDto>>();
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Tenant not found.", result.Message);
        }

        [Fact]
        public async Task GetTenantById_ReturnsForbidden_WhenUserIsNotSysAdmin()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.GetAsync("/api/v1/tenants/1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
