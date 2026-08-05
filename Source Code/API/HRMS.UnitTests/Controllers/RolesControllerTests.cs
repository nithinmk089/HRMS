using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class RolesControllerTests
    {
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _roleRepoMock = new Mock<IRoleRepository>();
            _controller = new RolesController(_roleRepoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithRoleId()
        {
            // Arrange
            var request = new CreateRoleRequest { TenantId = 1, RoleCode = "ADMIN", RoleName = "Administrator" };
            _roleRepoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(10L);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task AssignUser_ReturnsOk_OnSuccess()
        {
            // Arrange
            var roleId = 1L;
            var userId = 2L;
            var tenantId = 1L;
            _roleRepoMock.Setup(repo => repo.AssignToUserAsync(tenantId, userId, roleId, It.IsAny<long>())).ReturnsAsync(true);

            // Act
            var result = await _controller.AssignUser(roleId, userId, tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
