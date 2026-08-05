using System;
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
    public class OvertimeControllerTests
    {
        private readonly Mock<IOvertimeRepository> _overtimeRepoMock = new();
        private readonly OvertimeController _controller;

        public OvertimeControllerTests()
        {
            _controller = new OvertimeController(_overtimeRepoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithRequestId()
        {
            var req = new CreateOvertimeRequestRequest { TenantId = 1, EmployeeId = 10, RequestedHours = 4.5m };
            _overtimeRepoMock.Setup(repo => repo.CreateOvertimeRequestAsync(req)).ReturnsAsync(200L);

            var result = await _controller.Create(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(200L, response.Data);
        }

        [Fact]
        public async Task Search_ReturnsOk_WithList()
        {
            var list = new List<OvertimeRequestDto> { new() { OvertimeRequestId = 200L, EmployeeId = 10L } };
            _overtimeRepoMock.Setup(repo => repo.SearchOvertimeRequestsAsync(1L, 10L, "Pending")).ReturnsAsync(list);

            var result = await _controller.Search(1L, 10L, "Pending");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<OvertimeRequestDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task Approve_ReturnsOk_WithSuccess()
        {
            _overtimeRepoMock.Setup(repo => repo.ApproveOvertimeRequestAsync(200L, 1L, 1L, "Approved")).ReturnsAsync(true);

            var result = await _controller.Approve(200L, 1L, "Approved");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Reject_ReturnsOk_WithSuccess()
        {
            _overtimeRepoMock.Setup(repo => repo.RejectOvertimeRequestAsync(200L, 1L, 1L, "Too many hours")).ReturnsAsync(true);

            var result = await _controller.Reject(200L, 1L, "Too many hours");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
