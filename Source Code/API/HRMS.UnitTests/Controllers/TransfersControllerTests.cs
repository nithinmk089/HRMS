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
    public class TransfersControllerTests
    {
        private readonly Mock<IEmployeeTransferRepository> _transferRepoMock = new();
        private readonly TransfersController _controller;

        public TransfersControllerTests()
        {
            _controller = new TransfersController(_transferRepoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithNewTransferId()
        {
            var req = new CreateEmployeeTransferRequest { EmployeeId = 10L, ToDepartmentId = 2L, EffectiveDate = DateTime.UtcNow };
            _transferRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(40L);

            var result = await _controller.Create(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(40L, response.Data);
        }

        [Fact]
        public async Task Approve_ReturnsOk_WithSuccess()
        {
            _transferRepoMock.Setup(repo => repo.ApproveAsync(40L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Approve(40L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Complete_ReturnsOk_WithSuccess()
        {
            _transferRepoMock.Setup(repo => repo.CompleteAsync(40L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Complete(40L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetReport_ReturnsOk_WithTransferList()
        {
            var list = new List<EmployeeTransferDto> { new() { EmployeeTransferID = 40L, EmployeeID = 10L } };
            _transferRepoMock.Setup(repo => repo.GetTransferHistoryReportAsync(1L, 10L)).ReturnsAsync(list);

            var result = await _controller.GetReport(1L, 10L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeTransferDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
