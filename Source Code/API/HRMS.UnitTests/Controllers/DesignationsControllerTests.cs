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
    public class DesignationsControllerTests
    {
        private readonly Mock<IDesignationRepository> _repoMock;
        private readonly DesignationsController _controller;

        public DesignationsControllerTests()
        {
            _repoMock = new Mock<IDesignationRepository>();
            _controller = new DesignationsController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreateDesignationRequest { TenantId = 1, DesignationCode = "DESG1", DesignationName = "Developer" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(5L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(5L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var request = new UpdateDesignationRequest { DesignationId = 1, DesignationName = "Senior Developer" };
            _repoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
