using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class TenantsControllerTests
    {
        private readonly Mock<ITenantRepository> _tenantRepoMock;
        private readonly TenantsController _controller;

        public TenantsControllerTests()
        {
            _tenantRepoMock = new Mock<ITenantRepository>();
            _controller = new TenantsController(_tenantRepoMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenTenantExists()
        {
            // Arrange
            var tenantId = 1L;
            var tenantDto = new TenantDto { TenantId = tenantId, TenantName = "Test Tenant", Status = "Active" };
            _tenantRepoMock.Setup(repo => repo.GetByIdAsync(tenantId)).ReturnsAsync(tenantDto);

            // Act
            var result = await _controller.GetById(tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<TenantDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Test Tenant", response.Data.TenantName);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTenantDoesNotExist()
        {
            // Arrange
            var tenantId = 99L;
            _tenantRepoMock.Setup(repo => repo.GetByIdAsync(tenantId)).ReturnsAsync((TenantDto)null);

            // Act
            var result = await _controller.GetById(tenantId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<TenantDto>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Tenant not found.", response.Message);
        }

        [Fact]
        public async Task Create_ReturnsCreated_WithTenantId()
        {
            // Arrange
            var request = new CreateTenantRequest { TenantCode = "T01", TenantName = "New Tenant", Status = "Active" };
            _tenantRepoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(5L);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(5L, response.Data);
        }
    }
}
