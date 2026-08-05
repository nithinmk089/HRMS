using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class PermissionsControllerTests
    {
        private readonly Mock<IPermissionRepository> _repoMock;
        private readonly PermissionsController _controller;

        public PermissionsControllerTests()
        {
            _repoMock = new Mock<IPermissionRepository>();
            _controller = new PermissionsController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreatePermissionRequest { TenantId = 1, PermissionCode = "P1", PermissionName = "View Reports" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(8L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(8L, response.Data);
        }

        [Fact]
        public async Task AssignRole_ReturnsOk()
        {
            _repoMock.Setup(repo => repo.AssignToRoleAsync(1, 1, 1, It.IsAny<long>())).ReturnsAsync(true);

            var result = await _controller.AssignRole(1, 1, 1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
