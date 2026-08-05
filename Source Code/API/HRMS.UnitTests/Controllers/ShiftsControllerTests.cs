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
    public class ShiftsControllerTests
    {
        private readonly Mock<IShiftRepository> _shiftRepoMock = new();
        private readonly ShiftsController _controller;

        public ShiftsControllerTests()
        {
            _controller = new ShiftsController(_shiftRepoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithShiftId()
        {
            var req = new CreateShiftRequest { TenantId = 1, ShiftCode = "GS", ShiftName = "General", StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17) };
            _shiftRepoMock.Setup(repo => repo.CreateShiftAsync(req)).ReturnsAsync(10L);

            var result = await _controller.Create(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk_WithSuccess()
        {
            var req = new UpdateShiftRequest { TenantId = 1, ShiftCode = "GS2", ShiftName = "General v2" };
            _shiftRepoMock.Setup(repo => repo.UpdateShiftAsync(req)).ReturnsAsync(true);

            var result = await _controller.Update(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WithSuccess()
        {
            _shiftRepoMock.Setup(repo => repo.DeleteShiftAsync(10L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Delete(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            _shiftRepoMock.Setup(repo => repo.GetShiftByIdAsync(99L, 1L)).ReturnsAsync((ShiftDto?)null);

            var result = await _controller.GetById(99L, 1L);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Search_ReturnsOk_WithList()
        {
            var list = new List<ShiftDto> { new() { ShiftId = 10L, ShiftCode = "GS" } };
            _shiftRepoMock.Setup(repo => repo.SearchShiftsAsync(1L, "GS")).ReturnsAsync(list);

            var result = await _controller.Search(1L, "GS");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ShiftDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
