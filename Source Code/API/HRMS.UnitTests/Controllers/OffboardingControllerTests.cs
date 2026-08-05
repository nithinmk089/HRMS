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
    public class OffboardingControllerTests
    {
        private readonly Mock<IOffboardingRepository> _offboardingRepoMock = new();
        private readonly OffboardingController _controller;

        public OffboardingControllerTests()
        {
            _controller = new OffboardingController(_offboardingRepoMock.Object);
        }

        [Fact]
        public async Task CreateExitRequest_ReturnsOk_WithRequestId()
        {
            var req = new CreateExitRequest { TenantID = 1, EmployeeID = 10, ExitReason = "Resignation" };
            _offboardingRepoMock.Setup(repo => repo.CreateExitRequestAsync(req)).ReturnsAsync(25L);

            var result = await _controller.CreateExitRequest(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(25L, response.Data);
        }

        [Fact]
        public async Task UpdateExitRequest_ReturnsOk_WithSuccess()
        {
            var req = new UpdateExitRequest { TenantID = 1, ExitReason = "Health concerns" };
            _offboardingRepoMock.Setup(repo => repo.UpdateExitRequestAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateExitRequest(25L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task SubmitExitRequest_ReturnsOk_WithSuccess()
        {
            _offboardingRepoMock.Setup(repo => repo.SubmitExitRequestAsync(25L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.SubmitExitRequest(25L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task ApproveExitRequest_ReturnsOk_WithSuccess()
        {
            var req = new ApproveExitRequest { TenantID = 1, Remarks = "Approved" };
            _offboardingRepoMock.Setup(repo => repo.ApproveExitRequestAsync(req)).ReturnsAsync(200L);

            var result = await _controller.ApproveExitRequest(25L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(200L, response.Data);
        }

        [Fact]
        public async Task RejectExitRequest_ReturnsOk_WithSuccess()
        {
            var req = new RejectExitRequest { TenantID = 1, Remarks = "Rejected" };
            _offboardingRepoMock.Setup(repo => repo.RejectExitRequestAsync(req)).ReturnsAsync(201L);

            var result = await _controller.RejectExitRequest(25L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(201L, response.Data);
        }

        [Fact]
        public async Task CreateClearanceRequest_ReturnsOk_WithClearanceId()
        {
            var req = new CreateClearanceRequest { TenantID = 1, EmployeeID = 10 };
            _offboardingRepoMock.Setup(repo => repo.CreateClearanceRequestAsync(req)).ReturnsAsync(50L);

            var result = await _controller.CreateClearanceRequest(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(50L, response.Data);
        }

        [Fact]
        public async Task ApproveClearanceRequest_ReturnsOk_WithSuccess()
        {
            _offboardingRepoMock.Setup(repo => repo.ApproveClearanceRequestAsync(50L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ApproveClearanceRequest(50L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CompleteClearanceTask_ReturnsOk_WithSuccess()
        {
            _offboardingRepoMock.Setup(repo => repo.CompleteClearanceTaskAsync(5L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.CompleteClearanceTask(5L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateAssetReturn_ReturnsOk_WithAssetReturnId()
        {
            var req = new CreateAssetReturnRequest { TenantID = 1, AssetID = 3L };
            _offboardingRepoMock.Setup(repo => repo.CreateAssetReturnAsync(req)).ReturnsAsync(15L);

            var result = await _controller.CreateAssetReturn(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(15L, response.Data);
        }

        [Fact]
        public async Task VerifyAssetReturn_ReturnsOk_WithSuccess()
        {
            var req = new VerifyAssetReturnRequest { TenantID = 1, ReturnCondition = "Good" };
            _offboardingRepoMock.Setup(repo => repo.VerifyAssetReturnAsync(req)).ReturnsAsync(true);

            var result = await _controller.VerifyAssetReturn(15L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateKnowledgeTransfer_ReturnsOk_WithKtId()
        {
            var req = new CreateKnowledgeTransferRequest { TenantID = 1, EmployeeID = 10 };
            _offboardingRepoMock.Setup(repo => repo.CreateKnowledgeTransferAsync(req)).ReturnsAsync(30L);

            var result = await _controller.CreateKnowledgeTransfer(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(30L, response.Data);
        }

        [Fact]
        public async Task CompleteKnowledgeTransfer_ReturnsOk_WithSuccess()
        {
            _offboardingRepoMock.Setup(repo => repo.CompleteKnowledgeTransferAsync(30L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.CompleteKnowledgeTransfer(30L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GenerateExperienceLetter_ReturnsOk_WithLetterRequestId()
        {
            var req = new GenerateExperienceLetterRequest { TenantID = 1, EmployeeID = 10 };
            _offboardingRepoMock.Setup(repo => repo.GenerateExperienceLetterAsync(req)).ReturnsAsync(18L);

            var result = await _controller.GenerateExperienceLetter(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(18L, response.Data);
        }

        [Fact]
        public async Task CalculateFullAndFinalSettlement_ReturnsOk_WithSettlementId()
        {
            var req = new CalculateFFSRequest { TenantID = 1, EmployeeID = 10, SettlementAmount = 5000 };
            _offboardingRepoMock.Setup(repo => repo.CalculateFullAndFinalSettlementAsync(req)).ReturnsAsync(88L);

            var result = await _controller.CalculateFullAndFinalSettlement(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(88L, response.Data);
        }

        [Fact]
        public async Task ApproveFullAndFinalSettlement_ReturnsOk_WithSuccess()
        {
            _offboardingRepoMock.Setup(repo => repo.ApproveFullAndFinalSettlementAsync(88L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ApproveFullAndFinalSettlement(88L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CloseFullAndFinalSettlement_ReturnsOk_WithSuccess()
        {
            _offboardingRepoMock.Setup(repo => repo.CloseFullAndFinalSettlementAsync(88L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.CloseFullAndFinalSettlement(88L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetExitStatusReport_ReturnsOk_WithList()
        {
            var list = new List<ExitStatusReportDto> { new() { ExitRequestID = 25L } };
            _offboardingRepoMock.Setup(repo => repo.GetExitStatusReportAsync(1L)).ReturnsAsync(list);

            var result = await _controller.GetExitStatusReport(1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ExitStatusReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetClearanceStatusReport_ReturnsOk_WithList()
        {
            var list = new List<ClearanceStatusReportDto> { new() { ClearanceRequestID = 50L } };
            _offboardingRepoMock.Setup(repo => repo.GetClearanceStatusReportAsync(1L)).ReturnsAsync(list);

            var result = await _controller.GetClearanceStatusReport(1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ClearanceStatusReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetFullAndFinalSummaryReport_ReturnsOk_WithList()
        {
            var list = new List<FullAndFinalSummaryReportDto> { new() { FullAndFinalSettlementID = 88L } };
            _offboardingRepoMock.Setup(repo => repo.GetFullAndFinalSummaryReportAsync(1L)).ReturnsAsync(list);

            var result = await _controller.GetFullAndFinalSummaryReport(1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<FullAndFinalSummaryReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
