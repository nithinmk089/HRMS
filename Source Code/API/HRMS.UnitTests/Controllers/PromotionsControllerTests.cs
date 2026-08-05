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
    public class PromotionsControllerTests
    {
        private readonly Mock<IEmployeePromotionRepository> _promotionRepoMock = new();
        private readonly PromotionsController _controller;

        public PromotionsControllerTests()
        {
            _controller = new PromotionsController(_promotionRepoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithNewPromotionId()
        {
            var req = new CreateEmployeePromotionRequest { EmployeeId = 10L, NewDesignationId = 3L, EffectiveDate = DateTime.UtcNow };
            _promotionRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(50L);

            var result = await _controller.Create(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(50L, response.Data);
        }

        [Fact]
        public async Task Approve_ReturnsOk_WithSuccess()
        {
            _promotionRepoMock.Setup(repo => repo.ApproveAsync(50L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Approve(50L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Complete_ReturnsOk_WithSuccess()
        {
            _promotionRepoMock.Setup(repo => repo.CompleteAsync(50L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Complete(50L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetReport_ReturnsOk_WithPromotionList()
        {
            var list = new List<EmployeePromotionDto> { new() { EmployeePromotionID = 50L, EmployeeID = 10L } };
            _promotionRepoMock.Setup(repo => repo.GetPromotionHistoryReportAsync(1L, 10L)).ReturnsAsync(list);

            var result = await _controller.GetReport(1L, 10L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeePromotionDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
