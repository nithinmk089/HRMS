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
    public class OnboardingControllerTests
    {
        private readonly Mock<IOnboardingRepository> _onboardingRepoMock = new();
        private readonly OnboardingController _controller;

        public OnboardingControllerTests()
        {
            _controller = new OnboardingController(_onboardingRepoMock.Object);
        }

        [Fact]
        public async Task CreateWorkflow_ReturnsOk_WithWorkflowId()
        {
            var req = new CreateOnboardingWorkflowRequest { TenantID = 1, EmployeeID = 10, WorkflowCode = "WF01" };
            _onboardingRepoMock.Setup(repo => repo.CreateWorkflowAsync(req)).ReturnsAsync(15L);

            var result = await _controller.CreateWorkflow(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(15L, response.Data);
        }

        [Fact]
        public async Task UpdateWorkflow_ReturnsOk_WithSuccess()
        {
            var req = new UpdateOnboardingWorkflowRequest { TenantID = 1, WorkflowCode = "WF01" };
            _onboardingRepoMock.Setup(repo => repo.UpdateWorkflowAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateWorkflow(15L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task StartWorkflow_ReturnsOk_WithSuccess()
        {
            _onboardingRepoMock.Setup(repo => repo.StartWorkflowAsync(15L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.StartWorkflow(15L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CompleteWorkflow_ReturnsOk_WithSuccess()
        {
            _onboardingRepoMock.Setup(repo => repo.CompleteWorkflowAsync(15L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.CompleteWorkflow(15L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task SearchWorkflows_ReturnsOk_WithList()
        {
            var list = new List<OnboardingWorkflowDto> { new() { OnboardingWorkflowID = 15L } };
            _onboardingRepoMock.Setup(repo => repo.SearchWorkflowsAsync(1L, 10L, "Pending")).ReturnsAsync(list);

            var result = await _controller.SearchWorkflows(1L, 10L, "Pending");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<OnboardingWorkflowDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateTask_ReturnsOk_WithTaskId()
        {
            var req = new CreateOnboardingTaskRequest { TenantID = 1, TaskCode = "TS01", TaskName = "Accept Policies" };
            _onboardingRepoMock.Setup(repo => repo.CreateTaskAsync(req)).ReturnsAsync(8L);

            var result = await _controller.CreateTask(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(8L, response.Data);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOk_WithSuccess()
        {
            var req = new UpdateOnboardingTaskRequest { TenantID = 1, TaskCode = "TS01", TaskName = "Accept Policies v2" };
            _onboardingRepoMock.Setup(repo => repo.UpdateTaskAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateTask(8L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteTask_ReturnsOk_WithSuccess()
        {
            _onboardingRepoMock.Setup(repo => repo.DeleteTaskAsync(8L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteTask(8L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task AssignTask_ReturnsOk_WithAssignmentId()
        {
            var req = new AssignOnboardingTaskRequest { TenantID = 1, EmployeeID = 10 };
            _onboardingRepoMock.Setup(repo => repo.AssignTaskAsync(req)).ReturnsAsync(45L);

            var result = await _controller.AssignTask(8L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(45L, response.Data);
        }

        [Fact]
        public async Task CompleteTaskAssignment_ReturnsOk_WithSuccess()
        {
            _onboardingRepoMock.Setup(repo => repo.CompleteTaskAssignmentAsync(45L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.CompleteTaskAssignment(45L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateDocumentSubmission_ReturnsOk_WithSubmissionId()
        {
            var req = new CreateDocumentSubmissionRequest { TenantID = 1, EmployeeID = 10, DocumentType = "ID_Proof" };
            _onboardingRepoMock.Setup(repo => repo.CreateDocumentSubmissionAsync(req)).ReturnsAsync(110L);

            var result = await _controller.CreateDocumentSubmission(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(110L, response.Data);
        }

        [Fact]
        public async Task SearchDocumentSubmissions_ReturnsOk_WithList()
        {
            var list = new List<DocumentSubmissionDto> { new() { EmployeeDocumentSubmissionID = 110L } };
            _onboardingRepoMock.Setup(repo => repo.SearchDocumentSubmissionsAsync(1L, 10L)).ReturnsAsync(list);

            var result = await _controller.SearchDocumentSubmissions(1L, 10L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<DocumentSubmissionDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task ApproveDocumentVerification_ReturnsOk_WithVerificationId()
        {
            var req = new ApproveDocumentVerificationRequest { TenantID = 1, VerificationRemarks = "Approved" };
            _onboardingRepoMock.Setup(repo => repo.ApproveDocumentVerificationAsync(req)).ReturnsAsync(77L);

            var result = await _controller.ApproveDocumentVerification(110L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(77L, response.Data);
        }

        [Fact]
        public async Task RejectDocumentVerification_ReturnsOk_WithVerificationId()
        {
            var req = new RejectDocumentVerificationRequest { TenantID = 1, VerificationRemarks = "Rejected" };
            _onboardingRepoMock.Setup(repo => repo.RejectDocumentVerificationAsync(req)).ReturnsAsync(78L);

            var result = await _controller.RejectDocumentVerification(110L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(78L, response.Data);
        }

        [Fact]
        public async Task RecordPolicyAcceptance_ReturnsOk_WithAcceptanceId()
        {
            var req = new RecordPolicyAcceptanceRequest { TenantID = 1, EmployeeID = 10, AcceptanceVersion = "v1" };
            _onboardingRepoMock.Setup(repo => repo.RecordPolicyAcceptanceAsync(req)).ReturnsAsync(99L);

            var result = await _controller.RecordPolicyAcceptance(12L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(99L, response.Data);
        }

        [Fact]
        public async Task CreateESignature_ReturnsOk_WithSignatureId()
        {
            var req = new CreateESignatureRequest { TenantID = 1, EmployeeID = 10, SignatureHash = "HASH" };
            _onboardingRepoMock.Setup(repo => repo.CreateESignatureAsync(req)).ReturnsAsync(66L);

            var result = await _controller.CreateESignature(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(66L, response.Data);
        }

        [Fact]
        public async Task CreateEquipmentProvisioning_ReturnsOk_WithProvisioningId()
        {
            var req = new CreateEquipmentProvisioningRequest { TenantID = 1, EmployeeID = 10, AssetID = 5L };
            _onboardingRepoMock.Setup(repo => repo.CreateEquipmentProvisioningAsync(req)).ReturnsAsync(140L);

            var result = await _controller.CreateEquipmentProvisioning(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(140L, response.Data);
        }

        [Fact]
        public async Task CreateProbationReview_ReturnsOk_WithReviewId()
        {
            var req = new CreateProbationReviewRequest { TenantID = 1, EmployeeID = 10, ReviewStatus = "Confirmed" };
            _onboardingRepoMock.Setup(repo => repo.CreateProbationReviewAsync(req)).ReturnsAsync(200L);

            var result = await _controller.CreateProbationReview(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(200L, response.Data);
        }

        [Fact]
        public async Task GetOnboardingStatusReport_ReturnsOk_WithList()
        {
            var list = new List<OnboardingStatusReportDto> { new() { OnboardingWorkflowID = 15L } };
            _onboardingRepoMock.Setup(repo => repo.GetOnboardingStatusReportAsync(1L)).ReturnsAsync(list);

            var result = await _controller.GetOnboardingStatusReport(1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<OnboardingStatusReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetPendingTasksReport_ReturnsOk_WithList()
        {
            var list = new List<PendingTasksReportDto> { new() { OnboardingTaskAssignmentID = 45L } };
            _onboardingRepoMock.Setup(repo => repo.GetPendingTasksReportAsync(1L)).ReturnsAsync(list);

            var result = await _controller.GetPendingTasksReport(1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PendingTasksReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
