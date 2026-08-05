using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class EmploymentControllerTests
    {
        private readonly Mock<IEmployeeEmploymentRepository> _employmentRepoMock = new();
        private readonly EmploymentController _controller;

        public EmploymentControllerTests()
        {
            _controller = new EmploymentController(_employmentRepoMock.Object);
        }

        [Fact]
        public async Task GetByEmployeeId_ReturnsOk_WithEmploymentDetails()
        {
            var details = new EmployeeEmploymentDto { EmployeeID = 10L, EmploymentType = "FullTime", NoticePeriodDays = 30 };
            _employmentRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(details);

            var result = await _controller.GetByEmployeeId(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<EmployeeEmploymentDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("FullTime", response.Data.EmploymentType);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithNewEmploymentId()
        {
            var req = new CreateEmployeeEmploymentRequest { EmployeeId = 10L, CompanyId = 1L, JoiningDate = System.DateTime.UtcNow };
            _employmentRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(30L);

            var result = await _controller.Create(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(30L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeEmploymentRequest { EmployeeEmploymentId = 30L, CompanyId = 1L, JoiningDate = System.DateTime.UtcNow };
            _employmentRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.Update(30L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
