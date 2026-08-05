using System;
using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.DTOs.Auth;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthRepository> _authRepoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authRepoMock = new Mock<IAuthRepository>();
            _configMock = new Mock<IConfiguration>();

            // Setup default configuration values for JWT to prevent NullReferenceException
            _configMock.Setup(c => c["Jwt:Key"]).Returns("HRMSDevSecretKey_MustBe32CharactersLong!!");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("HRMS.API");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("HRMS.Client");
            _configMock.Setup(c => c["Jwt:ExpiryMinutes"]).Returns("60");

            _controller = new AuthController(_authRepoMock.Object, _configMock.Object);

            // Mock HttpContext for Request properties used in Login (IP and User-Agent)
            var context = new DefaultHttpContext();
            context.Request.Headers["User-Agent"] = "TestBrowser";
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginRequest { Email = "admin@hrms.com", Password = "password" };
            var authResponse = new AuthResponse
            {
                Success = true,
                User = new UserDto { UserId = 1, Email = "admin@hrms.com", FirstName = "Admin", LastName = "User", TenantId = 1 }
            };

            _authRepoMock.Setup(repo => repo.LoginAsync(
                request.Email,
                request.Password,
                "127.0.0.1",
                "TestBrowser")
            ).ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<AuthResponse>>(okResult.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data.AccessToken);
            Assert.NotNull(response.Data.RefreshToken);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequest { Email = "bad@hrms.com", Password = "wrong" };
            var authResponse = new AuthResponse
            {
                Success = false,
                Message = "Invalid credentials"
            };

            _authRepoMock.Setup(repo => repo.LoginAsync(
                request.Email,
                request.Password,
                "127.0.0.1",
                "TestBrowser")
            ).ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<ApiResponse<AuthResponse>>(unauthResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Invalid credentials", response.Message);
        }

        [Fact]
        public void ForgotPassword_ReturnsOk()
        {
            // Act
            var result = _controller.ForgotPassword("test@hrms.com");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Contains("test@hrms.com", response.Data);
        }
    }
}
