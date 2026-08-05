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
    public class AttendanceControllerTests
    {
        private readonly Mock<IAttendanceRepository> _attendanceRepoMock = new();
        private readonly AttendanceController _controller;

        public AttendanceControllerTests()
        {
            _controller = new AttendanceController(_attendanceRepoMock.Object);
        }

        [Fact]
        public async Task ClockIn_ReturnsOk_WithAttendanceId()
        {
            var req = new CreateAttendanceRequest { TenantId = 1, EmployeeId = 10, ShiftId = 2, AttendanceDate = DateTime.Today };
            _attendanceRepoMock.Setup(repo => repo.ClockInAsync(req)).ReturnsAsync(15L);

            var result = await _controller.ClockIn(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(15L, response.Data);
        }

        [Fact]
        public async Task ClockOut_ReturnsOk_WithSuccess()
        {
            var req = new UpdateAttendanceRequest { TenantId = 1, AttendanceId = 15L };
            _attendanceRepoMock.Setup(repo => repo.ClockOutAsync(req)).ReturnsAsync(true);

            var result = await _controller.ClockOut(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            _attendanceRepoMock.Setup(repo => repo.GetByIdAsync(99L, 1L)).ReturnsAsync((AttendanceDto?)null);

            var result = await _controller.GetById(99L, 1L);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Search_ReturnsOk_WithList()
        {
            var list = new List<AttendanceDto> { new() { AttendanceId = 15L, EmployeeId = 10L } };
            _attendanceRepoMock.Setup(repo => repo.SearchAsync(1L, 10L, null, null)).ReturnsAsync(list);

            var result = await _controller.Search(1L, 10L, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<AttendanceDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task Recalculate_ReturnsOk_WithSuccess()
        {
            var date = DateTime.Today;
            _attendanceRepoMock.Setup(repo => repo.RecalculateAsync(1L, 10L, date, 1L)).ReturnsAsync(true);

            var result = await _controller.Recalculate(1L, 10L, date);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateAdjustment_ReturnsOk_WithAdjustmentId()
        {
            var req = new CreateAttendanceAdjustmentRequest { TenantId = 1, AttendanceId = 15L, AdjustmentReason = "Missed swipe" };
            _attendanceRepoMock.Setup(repo => repo.CreateAdjustmentAsync(req)).ReturnsAsync(30L);

            var result = await _controller.CreateAdjustment(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(30L, response.Data);
        }

        [Fact]
        public async Task ApproveAdjustment_ReturnsOk_WithSuccess()
        {
            _attendanceRepoMock.Setup(repo => repo.ApproveAdjustmentAsync(30L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ApproveAdjustment(30L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task RejectAdjustment_ReturnsOk_WithSuccess()
        {
            _attendanceRepoMock.Setup(repo => repo.RejectAdjustmentAsync(30L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.RejectAdjustment(30L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateRegularization_ReturnsOk_WithRegularizationId()
        {
            var req = new CreateAttendanceRegularizationRequest { TenantId = 1, EmployeeId = 10, RequestedDate = DateTime.Today, Reason = "Forgot card" };
            _attendanceRepoMock.Setup(repo => repo.CreateRegularizationAsync(req)).ReturnsAsync(40L);

            var result = await _controller.CreateRegularization(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(40L, response.Data);
        }

        [Fact]
        public async Task ApproveRegularization_ReturnsOk_WithSuccess()
        {
            _attendanceRepoMock.Setup(repo => repo.ApproveRegularizationAsync(40L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ApproveRegularization(40L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task RejectRegularization_ReturnsOk_WithSuccess()
        {
            _attendanceRepoMock.Setup(repo => repo.RejectRegularizationAsync(40L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.RejectRegularization(40L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
