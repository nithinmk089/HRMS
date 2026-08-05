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
    public class EmploymentApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IEmployeeEmploymentRepository> _employmentRepoMock = new();

        public EmploymentApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeeEmploymentRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IEmployeeEmploymentRepository>(_ => _employmentRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetEmployment_ReturnsOk_WhenExists()
        {
            // Arrange
            var details = new EmployeeEmploymentDto { EmployeeID = 10L, EmploymentType = "Contractor" };
            _employmentRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(details);
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/employment/10?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeEmploymentDto>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Contractor", result.Data.EmploymentType);
        }

        [Fact]
        public async Task CreateEmployment_ReturnsForbidden_WhenNotAdmin()
        {
            // Arrange
            var request = new CreateEmployeeEmploymentRequest { EmployeeId = 10L, CompanyId = 1L, EmploymentType = "FullTime" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/employment", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
