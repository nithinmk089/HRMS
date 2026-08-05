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
    public class AssetApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IAssetRepository> _assetRepoMock = new();

        public AssetApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAssetRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IAssetRepository>(_ => _assetRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task SearchAssets_ReturnsSuccess_WhenUserIsAuthorized()
        {
            // Arrange
            var list = new List<AssetDto> { new AssetDto { AssetID = 1L, AssetCode = "AST-01", AssetName = "Laptop High", TenantID = 1 } };
            _assetRepoMock.Setup(repo => repo.SearchAssetsAsync(1L, "Laptop", null, 1, 50)).ReturnsAsync(list);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/assets?tenantId=1&searchText=Laptop");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<AssetDto>>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task CreateAsset_ReturnsForbidden_WhenUserIsNormalUser()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/assets", new CreateAssetRequest());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
