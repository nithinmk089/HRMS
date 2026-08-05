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
    public class EmployeeApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
        private readonly Mock<IEmployeeAddressRepository> _addressRepoMock = new();
        private readonly Mock<IEmployeeContactRepository> _contactRepoMock = new();
        private readonly Mock<IEmployeeEmergencyContactRepository> _emergencyContactRepoMock = new();

        public EmployeeApiTests(WebApplicationFactory<Program> factory)
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

                    // Remove existing registrations and replace with mocks
                    var empDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeeRepository));
                    if (empDescriptor != null) services.Remove(empDescriptor);
                    services.AddScoped<IEmployeeRepository>(_ => _employeeRepoMock.Object);

                    var addrDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeeAddressRepository));
                    if (addrDescriptor != null) services.Remove(addrDescriptor);
                    services.AddScoped<IEmployeeAddressRepository>(_ => _addressRepoMock.Object);

                    var contactDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeeContactRepository));
                    if (contactDescriptor != null) services.Remove(contactDescriptor);
                    services.AddScoped<IEmployeeContactRepository>(_ => _contactRepoMock.Object);

                    var emergencyDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmployeeEmergencyContactRepository));
                    if (emergencyDescriptor != null) services.Remove(emergencyDescriptor);
                    services.AddScoped<IEmployeeEmergencyContactRepository>(_ => _emergencyContactRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsSuccessWrapper_WhenExists()
        {
            // Arrange
            var empDto = new EmployeeDto { EmployeeID = 10L, FirstName = "Integration", LastName = "Employee" };
            _employeeRepoMock.Setup(repo => repo.GetByIdAsync(10L, 1L)).ReturnsAsync(empDto);
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/employees/10?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeDto>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Integration", result.Data.FirstName);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsCreated_WhenAuthorizedAsAdmin()
        {
            // Arrange
            var request = new CreateEmployeeRequest { TenantId = 1, FirstName = "Jane", LastName = "Smith", EmployeeCode = "EMP999", EmployeeNumber = "999" };
            _employeeRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<CreateEmployeeRequest>())).ReturnsAsync(15L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/employees", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(15L, result.Data);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsForbidden_WhenNotAdmin()
        {
            // Arrange
            var request = new CreateEmployeeRequest { TenantId = 1, FirstName = "Jane", LastName = "Smith" };
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/employees", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetAddresses_ReturnsAddresses_WhenValid()
        {
            // Arrange
            var list = new List<EmployeeAddressDto> { new() { EmployeeAddressID = 5L, AddressLine1 = "Test Street" } };
            _addressRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(list);
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/employees/10/addresses?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<EmployeeAddressDto>>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Single(result.Data);
        }
    }
}
