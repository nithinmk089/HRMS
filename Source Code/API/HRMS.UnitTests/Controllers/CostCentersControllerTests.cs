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
    public class CostCentersControllerTests
    {
        private readonly Mock<ICostCenterRepository> _repoMock;
        private readonly CostCentersController _controller;

        public CostCentersControllerTests()
        {
            _repoMock = new Mock<ICostCenterRepository>();
            _controller = new CostCentersController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreateCostCenterRequest { TenantId = 1, CostCenterCode = "CC1", CostCenterName = "R&D" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(7L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(7L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var request = new UpdateCostCenterRequest { CostCenterId = 1, CostCenterName = "Research" };
            _repoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
