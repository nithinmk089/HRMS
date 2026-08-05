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
    public class DepartmentsControllerTests
    {
        private readonly Mock<IDepartmentRepository> _repoMock;
        private readonly DepartmentsController _controller;

        public DepartmentsControllerTests()
        {
            _repoMock = new Mock<IDepartmentRepository>();
            _controller = new DepartmentsController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreateDepartmentRequest { TenantId = 1, BusinessUnitId = 1, DepartmentCode = "D1", DepartmentName = "Dept One" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(4L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(4L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var request = new UpdateDepartmentRequest { DepartmentId = 1, DepartmentName = "Dept Updated" };
            _repoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
