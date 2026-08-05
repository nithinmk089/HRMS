using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.DTOs.Auth;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HRMS.IntegrationTests
{
    public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IAuthRepository> _authRepoMock = new();

        public AuthApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<IAuthRepository>(_ => _authRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginRequest { Email = "admin@hrms.com", Password = "password" };
            var authResponse = new AuthResponse
            {
                Success = true,
                AccessToken = "sample-jwt-token",
                RefreshToken = "sample-refresh-token",
                User = new UserDto { UserId = 1, Email = "admin@hrms.com", FirstName = "Admin", LastName = "Operator" }
            };
            
            _authRepoMock.Setup(repo => repo.LoginAsync(
                request.Email, 
                request.Password, 
                It.IsAny<string>(), 
                It.IsAny<string>())
            ).ReturnsAsync(authResponse);

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/auth/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data.AccessToken);
            Assert.NotEmpty(result.Data.AccessToken);

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(result.Data.AccessToken);
            Assert.NotNull(jwtToken);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email || c.Type == "email");
            Assert.NotNull(emailClaim);
            Assert.Equal("admin@hrms.com", emailClaim.Value);
        }

        [Fact]
        public async Task ForgotPassword_ReturnsSuccessMessage()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/auth/forgot-password", "admin@hrms.com");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Contains("admin@hrms.com", result.Data);
        }
    }
}
