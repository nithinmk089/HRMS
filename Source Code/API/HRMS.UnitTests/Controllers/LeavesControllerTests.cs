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
    public class LeavesControllerTests
    {
        private readonly Mock<ILeaveRepository> _leaveRepoMock = new();
        private readonly LeavesController _controller;

        public LeavesControllerTests()
        {
            _controller = new LeavesController(_leaveRepoMock.Object);
        }

        [Fact]
        public async Task CreateLeaveType_ReturnsOk_WithTypeId()
        {
            var req = new CreateLeaveTypeRequest { TenantId = 1, LeaveCode = "AL", LeaveName = "Annual Leave" };
            _leaveRepoMock.Setup(repo => repo.CreateLeaveTypeAsync(req)).ReturnsAsync(5L);

            var result = await _controller.CreateLeaveType(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(5L, response.Data);
        }

        [Fact]
        public async Task SearchLeaveTypes_ReturnsOk_WithList()
        {
            var list = new List<LeaveTypeDto> { new() { LeaveTypeId = 5L, LeaveCode = "AL" } };
            _leaveRepoMock.Setup(repo => repo.SearchLeaveTypesAsync(1L, "AL")).ReturnsAsync(list);

            var result = await _controller.SearchLeaveTypes(1L, "AL");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<LeaveTypeDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateLeavePolicy_ReturnsOk_WithPolicyId()
        {
            var req = new CreateLeavePolicyRequest { TenantId = 1, PolicyName = "Standard Policy" };
            _leaveRepoMock.Setup(repo => repo.CreateLeavePolicyAsync(req)).ReturnsAsync(12L);

            var result = await _controller.CreateLeavePolicy(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(12L, response.Data);
        }

        [Fact]
        public async Task GetBalances_ReturnsOk_WithList()
        {
            var list = new List<LeaveBalanceDto> { new() { LeaveBalanceId = 2L, EmployeeId = 10L } };
            _leaveRepoMock.Setup(repo => repo.GetBalancesAsync(1L, 10L, 5L)).ReturnsAsync(list);

            var result = await _controller.GetBalances(10L, 1L, 5L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<LeaveBalanceDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateLeaveRequest_ReturnsOk_WithRequestId()
        {
            var req = new CreateLeaveRequestRequest { TenantId = 1, EmployeeId = 10 };
            _leaveRepoMock.Setup(repo => repo.CreateLeaveRequestAsync(req)).ReturnsAsync(100L);

            var result = await _controller.CreateLeaveRequest(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(100L, response.Data);
        }

        [Fact]
        public async Task SearchLeaveRequests_ReturnsOk_WithList()
        {
            var list = new List<LeaveRequestDto> { new() { LeaveRequestId = 100L } };
            _leaveRepoMock.Setup(repo => repo.SearchLeaveRequestsAsync(1L, 10L, "Pending")).ReturnsAsync(list);

            var result = await _controller.SearchLeaveRequests(1L, 10L, "Pending");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<LeaveRequestDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task ApproveLeaveRequest_ReturnsOk_WithSuccess()
        {
            _leaveRepoMock.Setup(repo => repo.ApproveLeaveRequestAsync(100L, 1L, 1L, "Looks fine")).ReturnsAsync(true);

            var result = await _controller.ApproveLeaveRequest(100L, 1L, "Looks fine");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task RejectLeaveRequest_ReturnsOk_WithSuccess()
        {
            _leaveRepoMock.Setup(repo => repo.RejectLeaveRequestAsync(100L, 1L, 1L, "Not allowed")).ReturnsAsync(true);

            var result = await _controller.RejectLeaveRequest(100L, 1L, "Not allowed");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CancelLeaveRequest_ReturnsOk_WithSuccess()
        {
            _leaveRepoMock.Setup(repo => repo.CancelLeaveRequestAsync(100L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.CancelLeaveRequest(100L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
