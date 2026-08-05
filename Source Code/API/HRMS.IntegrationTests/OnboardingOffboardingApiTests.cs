using System;
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
    public class OnboardingOffboardingApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IOnboardingRepository> _onboardingRepoMock = new();
        private readonly Mock<IOffboardingRepository> _offboardingRepoMock = new();

        public OnboardingOffboardingApiTests(WebApplicationFactory<Program> factory)
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

                    var onboardingDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IOnboardingRepository));
                    if (onboardingDescriptor != null) services.Remove(onboardingDescriptor);
                    services.AddScoped<IOnboardingRepository>(_ => _onboardingRepoMock.Object);

                    var offboardingDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IOffboardingRepository));
                    if (offboardingDescriptor != null) services.Remove(offboardingDescriptor);
                    services.AddScoped<IOffboardingRepository>(_ => _offboardingRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task CreateOnboardingWorkflow_ReturnsOk_WhenAdmin()
        {
            var request = new CreateOnboardingWorkflowRequest { TenantID = 1L, EmployeeID = 10L, WorkflowCode = "WF01", WorkflowName = "Standard Onboarding", StartDate = DateTime.Today, TargetCompletionDate = DateTime.Today.AddDays(7) };
            _onboardingRepoMock.Setup(repo => repo.CreateWorkflowAsync(It.IsAny<CreateOnboardingWorkflowRequest>())).ReturnsAsync(15L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsJsonAsync("/api/v1/onboarding/workflows", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(15L, result.Data);
        }

        [Fact]
        public async Task SubmitExitRequest_ReturnsOk()
        {
            var request = new CreateExitRequest { TenantID = 1L, EmployeeID = 10L, ResignationDate = DateTime.Today, LastWorkingDate = DateTime.Today.AddDays(30), ExitReason = "Career Growth" };
            _offboardingRepoMock.Setup(repo => repo.CreateExitRequestAsync(It.IsAny<CreateExitRequest>())).ReturnsAsync(25L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.PostAsJsonAsync("/api/v1/offboarding/exit-requests", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(25L, result.Data);
        }

        [Fact]
        public async Task StartOnboardingWorkflow_ReturnsOk()
        {
            _onboardingRepoMock.Setup(repo => repo.StartWorkflowAsync(15L, 1L, 1L)).ReturnsAsync(true);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsync("/api/v1/onboarding/workflows/15/start?tenantId=1", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task InitiateClearance_ReturnsOk()
        {
            var request = new CreateClearanceRequest { TenantID = 1L, EmployeeID = 10L };
            _offboardingRepoMock.Setup(repo => repo.CreateClearanceRequestAsync(It.IsAny<CreateClearanceRequest>())).ReturnsAsync(50L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsJsonAsync("/api/v1/offboarding/clearances", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(50L, result.Data);
        }

        [Fact]
        public async Task CalculateFullAndFinal_ReturnsOk()
        {
            var request = new CalculateFFSRequest { TenantID = 1L, EmployeeID = 10L, SettlementAmount = 5200m };
            _offboardingRepoMock.Setup(repo => repo.CalculateFullAndFinalSettlementAsync(It.IsAny<CalculateFFSRequest>())).ReturnsAsync(99L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsJsonAsync("/api/v1/offboarding/settlements/calculate", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(99L, result.Data);
        }
    }
}
