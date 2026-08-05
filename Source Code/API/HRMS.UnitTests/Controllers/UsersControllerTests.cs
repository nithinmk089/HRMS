using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _controller = new UsersController(_userRepoMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var userId = 1L;
            var tenantId = 1L;
            var userDto = new ApplicationUserDto { UserId = userId, UserName = "testuser", Email = "test@hrms.com", IsLocked = false };
            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId, tenantId)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetById(userId, tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<ApplicationUserDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("testuser", response.Data.UserName);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 99L;
            var tenantId = 1L;
            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId, tenantId)).ReturnsAsync((ApplicationUserDto)null);

            // Act
            var result = await _controller.GetById(userId, tenantId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<ApplicationUserDto>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User not found.", response.Message);
        }

        [Fact]
        public async Task Lock_ReturnsOk_OnSuccess()
        {
            // Arrange
            var userId = 1L;
            var tenantId = 1L;
            _userRepoMock.Setup(repo => repo.LockAsync(userId, tenantId, It.IsAny<long>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Lock(userId, tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
