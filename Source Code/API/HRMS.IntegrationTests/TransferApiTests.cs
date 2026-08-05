using System;
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
    public class TransferApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IEmployeeTransferRepository> _transferRepoMock = new();

        public TransferApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeeTransferRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IEmployeeTransferRepository>(_ => _transferRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task CreateTransfer_ReturnsOk_WhenAdmin()
        {
            // Arrange
            var request = new CreateEmployeeTransferRequest { EmployeeId = 10L, ToDepartmentId = 2L, EffectiveDate = DateTime.UtcNow };
            _transferRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<CreateEmployeeTransferRequest>())).ReturnsAsync(40L);
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/transfers", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(40L, result.Data);
        }

        [Fact]
        public async Task ApproveTransfer_ReturnsForbidden_WhenNotAdmin()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.PutAsync("/api/v1/transfers/40/approve?tenantId=1", null);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
