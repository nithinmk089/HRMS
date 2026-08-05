using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class TravelApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<ITravelRepository> _repoMock = new();

        public TravelApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITravelRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<ITravelRepository>(_ => _repoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetPolicies_ReturnsSuccess()
        {
            var list = new List<TravelPolicyDto> { new() { TravelPolicyID = 1L, PolicyCode = "STD", TenantID = 1 } };
            _repoMock.Setup(r => r.GetPoliciesAsync(1L)).ReturnsAsync(list);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.GetAsync("/api/v1/travel/policies?tenantId=1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<List<TravelPolicyDto>>>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);
            Assert.Single(apiResult.Data);
        }

        [Fact]
        public async Task CreatePolicy_ReturnsForbidden_WhenNotAdmin()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.PostAsJsonAsync("/api/v1/travel/policies", new CreateTravelPolicyRequest { TenantID = 1, PolicyCode = "STD", PolicyName = "Standard" });
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreatePolicy_ReturnsSuccess_WhenAdmin()
        {
            _repoMock.Setup(r => r.CreatePolicyAsync(It.IsAny<CreateTravelPolicyRequest>())).ReturnsAsync(1L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsJsonAsync("/api/v1/travel/policies", new CreateTravelPolicyRequest { TenantID = 1, PolicyCode = "STD", PolicyName = "Standard", EffectiveFrom = DateTime.UtcNow });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
