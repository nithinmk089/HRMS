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
    public class TimeAttendanceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IShiftRepository> _shiftRepoMock = new();
        private readonly Mock<IAttendanceRepository> _attendanceRepoMock = new();
        private readonly Mock<ILeaveRepository> _leaveRepoMock = new();
        private readonly Mock<IOvertimeRepository> _overtimeRepoMock = new();

        public TimeAttendanceApiTests(WebApplicationFactory<Program> factory)
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

                    var shiftDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IShiftRepository));
                    if (shiftDescriptor != null) services.Remove(shiftDescriptor);
                    services.AddScoped<IShiftRepository>(_ => _shiftRepoMock.Object);

                    var attDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAttendanceRepository));
                    if (attDescriptor != null) services.Remove(attDescriptor);
                    services.AddScoped<IAttendanceRepository>(_ => _attendanceRepoMock.Object);

                    var leaveDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ILeaveRepository));
                    if (leaveDescriptor != null) services.Remove(leaveDescriptor);
                    services.AddScoped<ILeaveRepository>(_ => _leaveRepoMock.Object);

                    var otDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IOvertimeRepository));
                    if (otDescriptor != null) services.Remove(otDescriptor);
                    services.AddScoped<IOvertimeRepository>(_ => _overtimeRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task CreateShift_ReturnsOk_WhenAdmin()
        {
            var request = new CreateShiftRequest { ShiftCode = "GS", ShiftName = "General", StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17) };
            _shiftRepoMock.Setup(repo => repo.CreateShiftAsync(It.IsAny<CreateShiftRequest>())).ReturnsAsync(1L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsJsonAsync("/api/v1/shifts", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(1L, result.Data);
        }

        [Fact]
        public async Task ClockIn_ReturnsOk()
        {
            var request = new CreateAttendanceRequest { EmployeeId = 1L, ShiftId = 1L, AttendanceDate = DateTime.Today, AttendanceStatus = "Present" };
            _attendanceRepoMock.Setup(repo => repo.ClockInAsync(It.IsAny<CreateAttendanceRequest>())).ReturnsAsync(10L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.PostAsJsonAsync("/api/v1/attendance/clock-in", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(10L, result.Data);
        }

        [Fact]
        public async Task SubmitLeaveRequest_ReturnsOk()
        {
            var request = new CreateLeaveRequestRequest { EmployeeId = 1L, LeaveTypeId = 2L, FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(2), TotalDays = 3 };
            _leaveRepoMock.Setup(repo => repo.CreateLeaveRequestAsync(It.IsAny<CreateLeaveRequestRequest>())).ReturnsAsync(100L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.PostAsJsonAsync("/api/v1/leaves/requests", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(100L, result.Data);
        }

        [Fact]
        public async Task CreateOvertime_ReturnsOk()
        {
            var request = new CreateOvertimeRequestRequest { EmployeeId = 1L, OvertimeDate = DateTime.Today, RequestedHours = 2.5m };
            _overtimeRepoMock.Setup(repo => repo.CreateOvertimeRequestAsync(It.IsAny<CreateOvertimeRequestRequest>())).ReturnsAsync(200L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.PostAsJsonAsync("/api/v1/overtime", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(200L, result.Data);
        }
    }
}
