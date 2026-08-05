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
    public class TravelControllerTests
    {
        private readonly Mock<ITravelRepository> _repoMock = new();
        private readonly TravelController _controller;

        public TravelControllerTests()
        {
            _controller = new TravelController(_repoMock.Object);
        }

        [Fact]
        public async Task GetPolicies_ReturnsOk()
        {
            var list = new List<TravelPolicyDto> { new() { TravelPolicyID = 1L, PolicyCode = "STD" } };
            _repoMock.Setup(r => r.GetPoliciesAsync(1L)).ReturnsAsync(list);
            var result = await _controller.GetPolicies(1L);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<TravelPolicyDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreatePolicy_ReturnsOk()
        {
            var req = new CreateTravelPolicyRequest { TenantID = 1, PolicyCode = "STD", PolicyName = "Standard Policy" };
            _repoMock.Setup(r => r.CreatePolicyAsync(req)).ReturnsAsync(10L);
            var result = await _controller.CreatePolicy(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task GetRequests_ReturnsOk()
        {
            var list = new List<TravelRequestDto> { new() { TravelRequestID = 1L, Destination = "New York" } };
            _repoMock.Setup(r => r.GetRequestsAsync(1L, null)).ReturnsAsync(list);
            var result = await _controller.GetRequests(1L, null);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<TravelRequestDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateRequest_ReturnsOk()
        {
            var req = new CreateTravelRequest { TenantID = 1, EmployeeID = 5, Destination = "Paris" };
            _repoMock.Setup(r => r.CreateRequestAsync(req)).ReturnsAsync(100L);
            var result = await _controller.CreateRequest(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(100L, response.Data);
        }

        [Fact]
        public async Task GetAdvances_ReturnsOk()
        {
            var list = new List<TravelAdvanceDto> { new() { TravelAdvanceID = 1L, AdvanceAmount = 500 } };
            _repoMock.Setup(r => r.GetAdvancesAsync(1L, null)).ReturnsAsync(list);
            var result = await _controller.GetAdvances(1L, null);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<TravelAdvanceDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateAdvance_ReturnsOk()
        {
            var req = new CreateTravelAdvanceRequest { TenantID = 1, TravelRequestID = 100, AdvanceAmount = 500 };
            _repoMock.Setup(r => r.CreateAdvanceAsync(req)).ReturnsAsync(200L);
            var result = await _controller.CreateAdvance(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(200L, response.Data);
        }

        [Fact]
        public async Task GetCategories_ReturnsOk()
        {
            var list = new List<ExpenseCategoryDto> { new() { ExpenseCategoryID = 1L, CategoryCode = "MEAL" } };
            _repoMock.Setup(r => r.GetCategoriesAsync(1L)).ReturnsAsync(list);
            var result = await _controller.GetCategories(1L);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ExpenseCategoryDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetClaims_ReturnsOk()
        {
            var list = new List<ExpenseClaimDto> { new() { ExpenseClaimID = 1L, TotalAmount = 150 } };
            _repoMock.Setup(r => r.GetClaimsAsync(1L, null)).ReturnsAsync(list);
            var result = await _controller.GetClaims(1L, null);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ExpenseClaimDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateClaim_ReturnsOk()
        {
            var req = new CreateExpenseClaimRequest { TenantID = 1, EmployeeID = 5, ClaimDate = DateTime.UtcNow };
            _repoMock.Setup(r => r.CreateClaimAsync(req)).ReturnsAsync(300L);
            var result = await _controller.CreateClaim(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(300L, response.Data);
        }

        [Fact]
        public async Task SubmitClaim_ReturnsOk()
        {
            var result = await _controller.SubmitClaim(300L, 1L);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task UploadReceipt_ReturnsOk()
        {
            _repoMock.Setup(r => r.UploadReceiptAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(1L);
            var result = await _controller.UploadReceipt(1L, 1L, "receipt.pdf");
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1L, response.Data);
        }

        [Fact]
        public async Task GetTransactions_ReturnsOk()
        {
            var list = new List<CorporateCardTransactionDto> { new() { CorporateCardTransactionID = 1L, MerchantName = "Delta" } };
            _repoMock.Setup(r => r.GetTransactionsAsync(1L, null)).ReturnsAsync(list);
            var result = await _controller.GetTransactions(1L, null);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CorporateCardTransactionDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task ReconcileTransaction_ReturnsOk()
        {
            var req = new ReconcileCorporateCardRequest { CorporateCardTransactionID = 1, ExpenseClaimItemID = 1, TenantID = 1 };
            var result = await _controller.ReconcileTransaction(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(response.Success);
        }
    }
}
