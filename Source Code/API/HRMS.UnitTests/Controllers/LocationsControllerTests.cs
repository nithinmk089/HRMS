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
    public class LocationsControllerTests
    {
        private readonly Mock<ILocationRepository> _repoMock;
        private readonly LocationsController _controller;

        public LocationsControllerTests()
        {
            _repoMock = new Mock<ILocationRepository>();
            _controller = new LocationsController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreateLocationRequest { TenantId = 1, LocationCode = "LOC1", LocationName = "HQ" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(6L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(6L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var request = new UpdateLocationRequest { LocationId = 1, LocationName = "HQ Main" };
            _repoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
