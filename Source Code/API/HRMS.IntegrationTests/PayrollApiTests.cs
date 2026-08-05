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
    public class PayrollApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IPayrollRepository> _payrollRepoMock = new();

        public PayrollApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPayrollRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IPayrollRepository>(_ => _payrollRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task SearchCalendars_ReturnsSuccess_WhenUserIsAuthorized()
        {
            var list = new List<PayrollCalendarDto> { new PayrollCalendarDto { PayrollCalendarID = 1L, CalendarCode = "STANDARD_2026", TenantID = 1 } };
            _payrollRepoMock.Setup(repo => repo.SearchCalendarsAsync(1L, "STANDARD", 1, 50)).ReturnsAsync(list);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.GetAsync("/api/v1/payroll/calendars?tenantId=1&searchText=STANDARD&page=1&pageSize=50");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<List<PayrollCalendarDto>>>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);
            Assert.Single(apiResult.Data);
            Assert.Equal("STANDARD_2026", apiResult.Data[0].CalendarCode);
        }

        [Fact]
        public async Task CreateCalendar_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            var req = new CreatePayrollCalendarRequest { TenantID = 1, CalendarCode = "STANDARD_2026", CalendarName = "Standard 2026" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER"); // Non-admin role

            var response = await client.PostAsJsonAsync("/api/v1/payroll/calendars", req);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateCalendar_ReturnsSuccess_WhenUserIsAdmin()
        {
            var req = new CreatePayrollCalendarRequest { TenantID = 1, CalendarCode = "STANDARD_2026", CalendarName = "Standard 2026" };
            _payrollRepoMock.Setup(repo => repo.CreateCalendarAsync(It.IsAny<CreatePayrollCalendarRequest>())).ReturnsAsync(1L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN"); // Admin role

            var response = await client.PostAsJsonAsync("/api/v1/payroll/calendars", req);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);
            Assert.Equal(1L, apiResult.Data);
        }
    }
}
