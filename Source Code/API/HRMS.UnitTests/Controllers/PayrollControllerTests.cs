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
    public class PayrollControllerTests
    {
        private readonly Mock<IPayrollRepository> _payrollRepoMock = new();
        private readonly PayrollController _controller;

        public PayrollControllerTests()
        {
            _controller = new PayrollController(_payrollRepoMock.Object);
        }

        [Fact]
        public async Task CreateCalendar_ReturnsOk_WithCalendarId()
        {
            var req = new CreatePayrollCalendarRequest { TenantID = 1, CalendarCode = "STANDARD_2026", CalendarName = "Standard 2026" };
            _payrollRepoMock.Setup(repo => repo.CreateCalendarAsync(req)).ReturnsAsync(1L);

            var result = await _controller.CreateCalendar(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1L, response.Data);
        }

        [Fact]
        public async Task SearchCalendars_ReturnsOk_WithCalendars()
        {
            var list = new List<PayrollCalendarDto> { new() { PayrollCalendarID = 1L, CalendarCode = "STANDARD_2026" } };
            _payrollRepoMock.Setup(repo => repo.SearchCalendarsAsync(1L, "STANDARD", 1, 50)).ReturnsAsync(list);

            var result = await _controller.SearchCalendars(1L, "STANDARD", 1, 50);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PayrollCalendarDto>>>(okResult.Value);
            Assert.True(response.Success);
            var data = Assert.Single(response.Data);
            Assert.Equal("STANDARD_2026", data.CalendarCode);
        }

        [Fact]
        public async Task CreatePeriod_ReturnsOk_WithPeriodId()
        {
            var req = new CreatePayrollPeriodRequest { TenantID = 1, PayrollCalendarID = 1L, PeriodCode = "JAN_2026" };
            _payrollRepoMock.Setup(repo => repo.CreatePeriodAsync(req)).ReturnsAsync(10L);

            var result = await _controller.CreatePeriod(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task CreateSalaryStructure_ReturnsOk_WithStructureId()
        {
            var req = new CreateSalaryStructureRequest { TenantID = 1, StructureCode = "EXEC_DIR" };
            _payrollRepoMock.Setup(repo => repo.CreateSalaryStructureAsync(req)).ReturnsAsync(15L);

            var result = await _controller.CreateSalaryStructure(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(15L, response.Data);
        }

        [Fact]
        public async Task CreatePayrollRun_ReturnsOk_WithRunId()
        {
            var req = new CreatePayrollRunRequest { TenantID = 1, PayrollPeriodID = 10L };
            _payrollRepoMock.Setup(repo => repo.CreatePayrollRunAsync(req)).ReturnsAsync(100L);

            var result = await _controller.CreatePayrollRun(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(100L, response.Data);
        }

        [Fact]
        public async Task ProcessRun_ReturnsOk_WithSuccess()
        {
            _payrollRepoMock.Setup(repo => repo.ProcessPayrollRunAsync(100L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ProcessRun(100L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateAdjustment_ReturnsOk_WithAdjustmentId()
        {
            var req = new CreatePayrollAdjustmentRequest { TenantID = 1, EmployeeID = 5L, PayrollPeriodID = 10L, AdjustmentAmount = 250 };
            _payrollRepoMock.Setup(repo => repo.CreateAdjustmentAsync(req)).ReturnsAsync(500L);

            var result = await _controller.CreateAdjustment(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(500L, response.Data);
        }

        [Fact]
        public async Task ApproveAdjustment_ReturnsOk_WithSuccess()
        {
            _payrollRepoMock.Setup(repo => repo.ApproveAdjustmentAsync(500L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ApproveAdjustment(500L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateLoan_ReturnsOk_WithLoanId()
        {
            var req = new CreateLoanRequest { TenantID = 1, EmployeeID = 5L, LoanType = "Personal", PrincipalAmount = 10000, TenureMonths = 12 };
            _payrollRepoMock.Setup(repo => repo.CreateLoanAdvanceAsync(req)).ReturnsAsync(600L);

            var result = await _controller.CreateLoan(req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(600L, response.Data);
        }

        [Fact]
        public async Task ApproveLoan_ReturnsOk_WithSuccess()
        {
            _payrollRepoMock.Setup(repo => repo.ApproveLoanAdvanceAsync(600L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.ApproveLoan(600L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
