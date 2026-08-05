using System.Collections.Generic;
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
    public class PerformanceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IPerformanceRepository> _performanceRepoMock = new();

        public PerformanceApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPerformanceRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IPerformanceRepository>(_ => _performanceRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetCycles_ReturnsSuccess_WhenUserIsAuthorized()
        {
            var list = new List<PerformanceCycleDto> { new PerformanceCycleDto { PerformanceCycleID = 1L, CycleCode = "CY_2026", TenantID = 1 } };
            _performanceRepoMock.Setup(repo => repo.GetCyclesAsync(1L)).ReturnsAsync(list);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.GetAsync("/api/v1/performance/cycles?tenantId=1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<List<PerformanceCycleDto>>>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);
            Assert.Single(apiResult.Data);
            Assert.Equal("CY_2026", apiResult.Data[0].CycleCode);
        }

        [Fact]
        public async Task CreateCycle_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            var req = new CreatePerformanceCycleRequest { TenantID = 1, CycleCode = "CY_2026", CycleName = "Annual Review 2026" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER"); // Non-admin role

            var response = await client.PostAsJsonAsync("/api/v1/performance/cycles", req);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateCycle_ReturnsSuccess_WhenUserIsAdmin()
        {
            var req = new CreatePerformanceCycleRequest { TenantID = 1, CycleCode = "CY_2026", CycleName = "Annual Review 2026" };
            _performanceRepoMock.Setup(repo => repo.CreateCycleAsync(It.IsAny<CreatePerformanceCycleRequest>())).ReturnsAsync(1L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN"); // Admin role

            var response = await client.PostAsJsonAsync("/api/v1/performance/cycles", req);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);
            Assert.Equal(1L, apiResult.Data);
        }
    }
}
