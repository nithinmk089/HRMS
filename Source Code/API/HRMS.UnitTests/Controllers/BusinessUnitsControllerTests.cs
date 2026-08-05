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
    public class BusinessUnitsControllerTests
    {
        private readonly Mock<IBusinessUnitRepository> _repoMock;
        private readonly BusinessUnitsController _controller;

        public BusinessUnitsControllerTests()
        {
            _repoMock = new Mock<IBusinessUnitRepository>();
            _controller = new BusinessUnitsController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreateBusinessUnitRequest { TenantId = 1, CompanyId = 1, BusinessUnitCode = "BU1", BusinessUnitName = "BU One" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(3L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(3L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var request = new UpdateBusinessUnitRequest { BusinessUnitId = 1, BusinessUnitName = "BU Updated" };
            _repoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            _repoMock.Setup(repo => repo.DeleteAsync(1, 1, 1)).ReturnsAsync(true);

            var result = await _controller.Delete(1, 1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
