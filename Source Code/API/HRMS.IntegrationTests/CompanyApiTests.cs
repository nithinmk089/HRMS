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
    public class CompanyApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<ICompanyRepository> _companyRepoMock = new();

        public CompanyApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICompanyRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<ICompanyRepository>(_ => _companyRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetCompanyById_ReturnsSuccess_WhenUserIsAdmin()
        {
            // Arrange
            var companyDto = new CompanyDto { CompanyId = 1L, CompanyName = "Integration Co", TenantId = 1 };
            _companyRepoMock.Setup(repo => repo.GetByIdAsync(1L, 1L)).ReturnsAsync(companyDto);
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.GetAsync("/api/v1/companies/1?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CompanyDto>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Integration Co", result.Data.CompanyName);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsForbidden_WhenUserIsNormalUser()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/companies/1?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
