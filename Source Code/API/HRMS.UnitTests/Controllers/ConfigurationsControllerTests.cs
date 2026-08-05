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
    public class ConfigurationsControllerTests
    {
        private readonly Mock<IConfigurationRepository> _repoMock;
        private readonly ConfigurationsController _controller;

        public ConfigurationsControllerTests()
        {
            _repoMock = new Mock<IConfigurationRepository>();
            _controller = new ConfigurationsController(_repoMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk_WithId()
        {
            var request = new CreateConfigurationRequest { TenantId = 1, ConfigurationKey = "K1", ConfigurationValue = "V1" };
            _repoMock.Setup(repo => repo.CreateAsync(request)).ReturnsAsync(9L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(9L, response.Data);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var request = new UpdateConfigurationRequest { ConfigurationId = 1, ConfigurationValue = "V2" };
            _repoMock.Setup(repo => repo.UpdateAsync(request)).ReturnsAsync(true);

            var result = await _controller.Update(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
            Assert.True(response.Data);
        }
    }
}
