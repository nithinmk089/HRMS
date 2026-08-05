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
    public class SelfServiceControllerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
        private readonly Mock<IEmployeeDocumentRepository> _documentRepoMock = new();
        private readonly Mock<IAttendanceRepository> _attendanceRepoMock = new();
        private readonly Mock<ILeaveRepository> _leaveRepoMock = new();
        private readonly SelfServiceController _controller;

        public SelfServiceControllerTests()
        {
            _controller = new SelfServiceController(
                _employeeRepoMock.Object, 
                _documentRepoMock.Object,
                _attendanceRepoMock.Object,
                _leaveRepoMock.Object);
        }

        [Fact]
        public async Task GetProfile_ReturnsOk_WithProfileDetails()
        {
            var emp = new EmployeeDto { EmployeeID = 10L, TenantID = 1L, FirstName = "John", LastName = "Doe" };
            _employeeRepoMock.Setup(repo => repo.GetByIdAsync(10L, 1L)).ReturnsAsync(emp);

            var result = await _controller.GetProfile(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<EmployeeDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("John", response.Data.FirstName);
        }

        [Fact]
        public async Task GetProfile_ReturnsNotFound_WhenProfileDoesNotExist()
        {
            _employeeRepoMock.Setup(repo => repo.GetByIdAsync(99L, 1L)).ReturnsAsync((EmployeeDto?)null);

            var result = await _controller.GetProfile(99L, 1L);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<EmployeeDto>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Profile not found.", response.Message);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeRequest { TenantId = 1L, PreferredName = "Johnny", MaritalStatus = "Single" };
            _employeeRepoMock.Setup(repo => repo.UpdateProfileAsync(10L, 1L, "Johnny", "Single", 1L)).ReturnsAsync(true);

            var result = await _controller.UpdateProfile(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task UpdateContactDetails_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeRequest { TenantId = 1L, PersonalEmail = "test@test.com", MobileNumber = "12345" };
            _employeeRepoMock.Setup(repo => repo.UpdateContactDetailsAsync(10L, 1L, "test@test.com", "12345", 1L)).ReturnsAsync(true);

            var result = await _controller.UpdateContactDetails(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetServiceHistory_ReturnsOk_WithHistory()
        {
            var list = new List<EmployeeServiceHistoryReportDto> { new() { RecordID = 10L, RecordType = "StatusChange" } };
            _employeeRepoMock.Setup(repo => repo.GetServiceHistoryReportAsync(1L, 10L)).ReturnsAsync(list);

            var result = await _controller.GetServiceHistory(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeServiceHistoryReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetDocuments_ReturnsOk_WithDocuments()
        {
            var list = new List<EmployeeDocumentDto> { new() { EmployeeDocumentID = 6L, DocumentType = "Visa" } };
            _documentRepoMock.Setup(repo => repo.SearchAsync(1L, 10L, "Visa")).ReturnsAsync(list);

            var result = await _controller.GetDocuments(10L, 1L, "Visa");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeDocumentDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task UploadDocument_ReturnsOk_WithNewDocumentId()
        {
            var req = new UploadEmployeeDocumentRequest { DocumentType = "Visa", FileName = "visa.pdf", FilePath = "/files/visa.pdf" };
            _documentRepoMock.Setup(repo => repo.UploadAsync(req)).ReturnsAsync(6L);

            var result = await _controller.UploadDocument(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(6L, response.Data);
        }
    }
}
