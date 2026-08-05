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
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
        private readonly Mock<IEmployeeAddressRepository> _addressRepoMock = new();
        private readonly Mock<IEmployeeContactRepository> _contactRepoMock = new();
        private readonly Mock<IEmployeeEmergencyContactRepository> _emergencyContactRepoMock = new();
        private readonly Mock<IEmployeeQualificationRepository> _qualificationRepoMock = new();
        private readonly Mock<IEmployeeCertificationRepository> _certificationRepoMock = new();
        private readonly Mock<IEmployeeDocumentRepository> _documentRepoMock = new();
        private readonly Mock<IEmployeeManagerRepository> _managerRepoMock = new();

        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _controller = new EmployeesController(
                _employeeRepoMock.Object,
                _addressRepoMock.Object,
                _contactRepoMock.Object,
                _emergencyContactRepoMock.Object,
                _qualificationRepoMock.Object,
                _certificationRepoMock.Object,
                _documentRepoMock.Object,
                _managerRepoMock.Object
            );
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithEmployeeId()
        {
            var req = new CreateEmployeeRequest { TenantId = 1, FirstName = "John", LastName = "Doe", EmployeeCode = "EMP001", EmployeeNumber = "12345" };
            _employeeRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(10L);

            var result = await _controller.Create(req);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk_WithSuccessResult()
        {
            var req = new UpdateEmployeeRequest { TenantId = 1, FirstName = "John", LastName = "Doe" };
            _employeeRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.Update(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WithSuccessResult()
        {
            _employeeRepoMock.Setup(repo => repo.DeleteAsync(10L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Delete(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            var emp = new EmployeeDto { EmployeeID = 10L, TenantID = 1, FirstName = "John", LastName = "Doe" };
            _employeeRepoMock.Setup(repo => repo.GetByIdAsync(10L, 1L)).ReturnsAsync(emp);

            var result = await _controller.GetById(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<EmployeeDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("John", response.Data.FirstName);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenDoesNotExist()
        {
            _employeeRepoMock.Setup(repo => repo.GetByIdAsync(99L, 1L)).ReturnsAsync((EmployeeDto?)null);

            var result = await _controller.GetById(99L, 1L);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<EmployeeDto>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Employee not found.", response.Message);
        }

        [Fact]
        public async Task Search_ReturnsOk_WithEmployeeList()
        {
            var list = new List<EmployeeDirectoryDto>
            {
                new() { EmployeeID = 10L, FirstName = "John", LastName = "Doe" }
            };
            _employeeRepoMock.Setup(repo => repo.SearchAsync(1L, "John", "Active", 1, 50)).ReturnsAsync(list);

            var result = await _controller.Search(1L, "John", "Active", 1, 50);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeDirectoryDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task Activate_ReturnsOk_WithSuccess()
        {
            _employeeRepoMock.Setup(repo => repo.ActivateAsync(10L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.Activate(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Suspend_ReturnsOk_WithSuccess()
        {
            _employeeRepoMock.Setup(repo => repo.SuspendAsync(10L, 1L, "reason", 1L)).ReturnsAsync(true);

            var result = await _controller.Suspend(10L, 1L, "reason");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Terminate_ReturnsOk_WithSuccess()
        {
            _employeeRepoMock.Setup(repo => repo.TerminateAsync(10L, 1L, "reason", 1L)).ReturnsAsync(true);

            var result = await _controller.Terminate(10L, 1L, "reason");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Rehire_ReturnsOk_WithSuccess()
        {
            _employeeRepoMock.Setup(repo => repo.RehireAsync(10L, 1L, "reason", 1L)).ReturnsAsync(true);

            var result = await _controller.Rehire(10L, 1L, "reason");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task UpdateContactDetails_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeRequest { TenantId = 1, PersonalEmail = "test@test.com", MobileNumber = "1234" };
            _employeeRepoMock.Setup(repo => repo.UpdateContactDetailsAsync(10L, 1L, "test@test.com", "1234", 1L)).ReturnsAsync(true);

            var result = await _controller.UpdateContactDetails(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeRequest { TenantId = 1, PreferredName = "Johnny", MaritalStatus = "Married" };
            _employeeRepoMock.Setup(repo => repo.UpdateProfileAsync(10L, 1L, "Johnny", "Married", 1L)).ReturnsAsync(true);

            var result = await _controller.UpdateProfile(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetAddresses_ReturnsOk_WithAddresses()
        {
            var list = new List<EmployeeAddressDto> { new() { EmployeeAddressID = 5L, AddressLine1 = "Street 1" } };
            _addressRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetAddresses(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeAddressDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateAddress_ReturnsOk_WithId()
        {
            var req = new CreateEmployeeAddressRequest { AddressLine1 = "Street 1", City = "City", Country = "Country" };
            _addressRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(5L);

            var result = await _controller.CreateAddress(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(5L, response.Data);
        }

        [Fact]
        public async Task UpdateAddress_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeAddressRequest { City = "CityUpdated" };
            _addressRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateAddress(10L, 5L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteAddress_ReturnsOk_WithSuccess()
        {
            _addressRepoMock.Setup(repo => repo.DeleteAsync(5L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteAddress(10L, 5L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetContacts_ReturnsOk_WithContacts()
        {
            var list = new List<EmployeeContactDto> { new() { EmployeeContactID = 2L, ContactValue = "1234567" } };
            _contactRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetContacts(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeContactDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateContact_ReturnsOk_WithId()
        {
            var req = new CreateEmployeeContactRequest { ContactType = "Mobile", ContactValue = "1234567" };
            _contactRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(2L);

            var result = await _controller.CreateContact(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(2L, response.Data);
        }

        [Fact]
        public async Task UpdateContact_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeContactRequest { ContactValue = "7654321" };
            _contactRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateContact(10L, 2L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteContact_ReturnsOk_WithSuccess()
        {
            _contactRepoMock.Setup(repo => repo.DeleteAsync(2L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteContact(10L, 2L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetEmergencyContacts_ReturnsOk_WithContacts()
        {
            var list = new List<EmployeeEmergencyContactDto> { new() { EmployeeEmergencyContactID = 3L, ContactName = "Jane" } };
            _emergencyContactRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetEmergencyContacts(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeEmergencyContactDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateEmergencyContact_ReturnsOk_WithId()
        {
            var req = new CreateEmployeeEmergencyContactRequest { ContactName = "Jane", Relationship = "Spouse", MobileNumber = "999" };
            _emergencyContactRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(3L);

            var result = await _controller.CreateEmergencyContact(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(3L, response.Data);
        }

        [Fact]
        public async Task UpdateEmergencyContact_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeEmergencyContactRequest { ContactName = "Jane Updated" };
            _emergencyContactRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateEmergencyContact(10L, 3L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteEmergencyContact_ReturnsOk_WithSuccess()
        {
            _emergencyContactRepoMock.Setup(repo => repo.DeleteAsync(3L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteEmergencyContact(10L, 3L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetQualifications_ReturnsOk_WithQualifications()
        {
            var list = new List<EmployeeQualificationDto> { new() { EmployeeQualificationID = 4L, QualificationType = "B.Sc" } };
            _qualificationRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetQualifications(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeQualificationDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateQualification_ReturnsOk_WithId()
        {
            var req = new CreateEmployeeQualificationRequest { QualificationType = "B.Sc", Institution = "Uni", YearOfPassing = 2020 };
            _qualificationRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(4L);

            var result = await _controller.CreateQualification(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(4L, response.Data);
        }

        [Fact]
        public async Task UpdateQualification_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeQualificationRequest { Institution = "Uni Updated" };
            _qualificationRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateQualification(10L, 4L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteQualification_ReturnsOk_WithSuccess()
        {
            _qualificationRepoMock.Setup(repo => repo.DeleteAsync(4L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteQualification(10L, 4L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetCertifications_ReturnsOk_WithCertifications()
        {
            var list = new List<EmployeeCertificationDto> { new() { EmployeeCertificationID = 5L, CertificationName = "AWS Cloud Practitioner" } };
            _certificationRepoMock.Setup(repo => repo.GetByEmployeeIdAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetCertifications(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeCertificationDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateCertification_ReturnsOk_WithId()
        {
            var req = new CreateEmployeeCertificationRequest { CertificationName = "AWS", CertificationAuthority = "Amazon", IssueDate = DateTime.UtcNow };
            _certificationRepoMock.Setup(repo => repo.CreateAsync(req)).ReturnsAsync(5L);

            var result = await _controller.CreateCertification(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(5L, response.Data);
        }

        [Fact]
        public async Task UpdateCertification_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeCertificationRequest { CertificationAuthority = "Amazon Updated" };
            _certificationRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateCertification(10L, 5L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteCertification_ReturnsOk_WithSuccess()
        {
            _certificationRepoMock.Setup(repo => repo.DeleteAsync(5L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteCertification(10L, 5L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetDocuments_ReturnsOk_WithDocuments()
        {
            var list = new List<EmployeeDocumentDto> { new() { EmployeeDocumentID = 6L, DocumentType = "Passport" } };
            _documentRepoMock.Setup(repo => repo.SearchAsync(1L, 10L, "Passport")).ReturnsAsync(list);

            var result = await _controller.GetDocuments(10L, 1L, "Passport");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeDocumentDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task UploadDocument_ReturnsOk_WithId()
        {
            var req = new UploadEmployeeDocumentRequest { DocumentType = "Passport", FileName = "passport.pdf", FilePath = "/files/passport.pdf" };
            _documentRepoMock.Setup(repo => repo.UploadAsync(req)).ReturnsAsync(6L);

            var result = await _controller.UploadDocument(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(6L, response.Data);
        }

        [Fact]
        public async Task UpdateDocument_ReturnsOk_WithSuccess()
        {
            var req = new UpdateEmployeeDocumentRequest { FileName = "passport_new.pdf" };
            _documentRepoMock.Setup(repo => repo.UpdateAsync(req)).ReturnsAsync(true);

            var result = await _controller.UpdateDocument(10L, 6L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task DeleteDocument_ReturnsOk_WithSuccess()
        {
            _documentRepoMock.Setup(repo => repo.DeleteAsync(6L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.DeleteDocument(10L, 6L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task CreateDocumentVersion_ReturnsOk_WithVersionId()
        {
            _documentRepoMock.Setup(repo => repo.CreateVersionAsync(6L, 1L, "v2.pdf", "/files/v2.pdf", "application/pdf", 2, 1L)).ReturnsAsync(15L);

            var result = await _controller.CreateDocumentVersion(10L, 6L, 1L, "v2.pdf", "/files/v2.pdf", "application/pdf", 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(15L, response.Data);
        }

        [Fact]
        public async Task RestoreDocumentVersion_ReturnsOk_WithSuccess()
        {
            _documentRepoMock.Setup(repo => repo.RestoreVersionAsync(6L, 1L, 2, 1L)).ReturnsAsync(true);

            var result = await _controller.RestoreDocumentVersion(10L, 6L, 2, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetManagers_ReturnsOk_WithManagers()
        {
            var list = new List<EmployeeManagerDto> { new() { EmployeeManagerID = 7L, ManagerID = 8L, ManagerFullName = "Boss" } };
            _managerRepoMock.Setup(repo => repo.GetManagersAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetManagers(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeManagerDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task AssignManager_ReturnsOk_WithSuccess()
        {
            var req = new AssignManagerRequest { ManagerId = 8L, EffectiveFrom = DateTime.UtcNow };
            _managerRepoMock.Setup(repo => repo.AssignAsync(req)).ReturnsAsync(true);

            var result = await _controller.AssignManager(10L, req);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task RemoveManager_ReturnsOk_WithSuccess()
        {
            _managerRepoMock.Setup(repo => repo.RemoveAsync(10L, 8L, 1L, 1L)).ReturnsAsync(true);

            var result = await _controller.RemoveManager(10L, 8L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task GetDirectReports_ReturnsOk_WithEmployees()
        {
            var list = new List<EmployeeDirectoryDto> { new() { EmployeeID = 11L, FirstName = "Junior" } };
            _managerRepoMock.Setup(repo => repo.GetDirectReportsAsync(10L, 1L)).ReturnsAsync(list);

            var result = await _controller.GetDirectReports(10L, 1L);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeDirectoryDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
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
        public async Task GetDirectoryReport_ReturnsOk_WithReport()
        {
            var list = new List<EmployeeDirectoryDto> { new() { EmployeeID = 10L, FirstName = "John" } };
            _employeeRepoMock.Setup(repo => repo.GetDirectoryReportAsync(1L, 2L, 3L, "Active")).ReturnsAsync(list);

            var result = await _controller.GetDirectoryReport(1L, 2L, 3L, "Active");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<EmployeeDirectoryDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetCertificationExpiry_ReturnsOk_WithReport()
        {
            var list = new List<CertificationExpiryReportDto> { new() { EmployeeID = 10L, DocumentName = "AWS" } };
            _certificationRepoMock.Setup(repo => repo.GetCertificationExpiryReportAsync(1L, 30)).ReturnsAsync(list);

            var result = await _controller.GetCertificationExpiry(1L, 30);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CertificationExpiryReportDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }
    }
}
