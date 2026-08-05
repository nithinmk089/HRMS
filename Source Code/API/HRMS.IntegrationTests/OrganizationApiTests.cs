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
    public class OrganizationApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IBusinessUnitRepository> _buMock = new();
        private readonly Mock<IDepartmentRepository> _deptMock = new();

        public OrganizationApiTests(WebApplicationFactory<Program> factory)
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

                    var buDesc = services.SingleOrDefault(d => d.ServiceType == typeof(IBusinessUnitRepository));
                    if (buDesc != null) services.Remove(buDesc);
                    services.AddScoped<IBusinessUnitRepository>(_ => _buMock.Object);

                    var deptDesc = services.SingleOrDefault(d => d.ServiceType == typeof(IDepartmentRepository));
                    if (deptDesc != null) services.Remove(deptDesc);
                    services.AddScoped<IDepartmentRepository>(_ => _deptMock.Object);
                });
            });
        }

        [Fact]
        public async Task SearchBusinessUnits_AllowedForNormalUser()
        {
            // Arrange
            _buMock.Setup(repo => repo.SearchAsync(1, null, null, 1, 50))
                .ReturnsAsync(new System.Collections.Generic.List<BusinessUnitDto>
                {
                    new BusinessUnitDto { BusinessUnitId = 1, BusinessUnitName = "Engineering", TenantId = 1 }
                });

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/business-units?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateBusinessUnit_ForbiddenForNormalUser()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var request = new CreateBusinessUnitRequest { TenantId = 1, CompanyId = 1, BusinessUnitCode = "BU-TEST", BusinessUnitName = "Test BU" };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/business-units", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateBusinessUnit_AllowedForAdmin()
        {
            // Arrange
            var request = new CreateBusinessUnitRequest { TenantId = 1, CompanyId = 1, BusinessUnitCode = "BU-TEST", BusinessUnitName = "Test BU" };
            _buMock.Setup(repo => repo.CreateAsync(It.IsAny<CreateBusinessUnitRequest>())).ReturnsAsync(1L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/business-units", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
