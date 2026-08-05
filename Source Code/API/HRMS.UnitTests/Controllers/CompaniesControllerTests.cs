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
    public class CompaniesControllerTests
    {
        private readonly Mock<ICompanyRepository> _companyRepoMock;
        private readonly CompaniesController _controller;

        public CompaniesControllerTests()
        {
            _companyRepoMock = new Mock<ICompanyRepository>();
            _controller = new CompaniesController(_companyRepoMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenCompanyExists()
        {
            // Arrange
            var companyId = 1L;
            var tenantId = 1L;
            var companyDto = new CompanyDto { CompanyId = companyId, CompanyName = "Test Company", TenantId = tenantId };
            _companyRepoMock.Setup(repo => repo.GetByIdAsync(companyId, tenantId)).ReturnsAsync(companyDto);

            // Act
            var result = await _controller.GetById(companyId, tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<CompanyDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Test Company", response.Data.CompanyName);
        }

        [Fact]
        public async Task Create_ReturnsCreated_WithCompanyId()
        {
            // Arrange
            var request = new CreateCompanyRequest { TenantId = 1, CompanyCode = "C01", CompanyName = "New Company" };
            _companyRepoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(2L);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(2L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk_WithTrue()
        {
            // Arrange
            var companyId = 1L;
            var request = new UpdateCompanyRequest { CompanyId = companyId, CompanyName = "Updated Name" };
            _companyRepoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(companyId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WithTrue()
        {
            // Arrange
            var companyId = 1L;
            var tenantId = 1L;
            _companyRepoMock.Setup(repo => repo.DeleteAsync(companyId, tenantId, 1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(companyId, tenantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
