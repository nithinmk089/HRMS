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
    public class PerformanceControllerTests
    {
        private readonly Mock<IPerformanceRepository> _performanceRepoMock = new();
        private readonly PerformanceController _controller;

        public PerformanceControllerTests()
        {
            _controller = new PerformanceController(_performanceRepoMock.Object);
        }

        [Fact]
        public async Task CreateCycle_ReturnsOk_WithCycleId()
        {
            var req = new CreatePerformanceCycleRequest { TenantID = 1, CycleCode = "CY_2026", CycleName = "Annual Review 2026" };
            _performanceRepoMock.Setup(repo => repo.CreateCycleAsync(req)).ReturnsAsync(1L);

            var result = await _controller.CreateCycle(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1L, response.Data);
        }

        [Fact]
        public async Task GetCycles_ReturnsOk_WithCyclesList()
        {
            var list = new List<PerformanceCycleDto> { new() { PerformanceCycleID = 1L, CycleCode = "CY_2026" } };
            _performanceRepoMock.Setup(repo => repo.GetCyclesAsync(1L)).ReturnsAsync(list);

            var result = await _controller.GetCycles(1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PerformanceCycleDto>>>(okResult.Value);
            Assert.True(response.Success);
            var data = Assert.Single(response.Data);
            Assert.Equal("CY_2026", data.CycleCode);
        }

        [Fact]
        public async Task CreateGoal_ReturnsOk_WithGoalId()
        {
            var req = new CreateGoalRequest { TenantID = 1, EmployeeID = 5L, PerformanceCycleID = 1L, GoalTitle = "Learn Angular" };
            _performanceRepoMock.Setup(repo => repo.CreateGoalAsync(req)).ReturnsAsync(10L);

            var result = await _controller.CreateGoal(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task LogGoalProgress_ReturnsOk_WithProgressId()
        {
            var req = new CreateGoalProgressRequest { TenantID = 1, GoalID = 10L, ProgressPercentage = 50 };
            _performanceRepoMock.Setup(repo => repo.CreateGoalProgressAsync(req)).ReturnsAsync(20L);

            var result = await _controller.LogGoalProgress(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(20L, response.Data);
        }

        [Fact]
        public async Task CreateFeedback_ReturnsOk_WithFeedbackId()
        {
            var req = new CreateFeedbackRequest { TenantID = 1, EmployeeID = 5L, FeedbackText = "Great work!" };
            _performanceRepoMock.Setup(repo => repo.CreateFeedbackAsync(req)).ReturnsAsync(30L);

            var result = await _controller.CreateFeedback(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(30L, response.Data);
        }
    }
}
