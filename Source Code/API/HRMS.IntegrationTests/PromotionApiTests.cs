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
    public class PromotionApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IEmployeePromotionRepository> _promotionRepoMock = new();

        public PromotionApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeePromotionRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IEmployeePromotionRepository>(_ => _promotionRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task CreatePromotion_ReturnsOk_WhenAdmin()
        {
            // Arrange
            var request = new CreateEmployeePromotionRequest { EmployeeId = 10L, NewDesignationId = 3L, EffectiveDate = DateTime.UtcNow };
            _promotionRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<CreateEmployeePromotionRequest>())).ReturnsAsync(50L);
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/promotions", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(50L, result.Data);
        }

        [Fact]
        public async Task CompletePromotion_ReturnsForbidden_WhenNotAdmin()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.PutAsync("/api/v1/promotions/50/complete?tenantId=1", null);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
