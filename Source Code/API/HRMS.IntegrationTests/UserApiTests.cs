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
    public class UserApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IUserRepository> _userRepoMock = new();

        public UserApiTests(WebApplicationFactory<Program> factory)
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

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IUserRepository>(_ => _userRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetUserById_ReturnsSuccessWrapper_WhenExists()
        {
            // Arrange
            var userDto = new ApplicationUserDto { UserID = 1L, UserName = "integrationuser", Email = "integration@hrms.com", IsLocked = false };
            _userRepoMock.Setup(repo => repo.GetByIdAsync(1L, 1L)).ReturnsAsync(userDto);
            
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/users/1?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ApplicationUserDto>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("integrationuser", result.Data.UserName);
        }

        [Fact]
        public async Task GetUserById_ReturnsForbidden_WhenUserIsNotAdminOrSysAdmin()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            // Act
            var response = await client.GetAsync("/api/v1/users/1?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
