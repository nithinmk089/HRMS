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
    public class AssetsControllerTests
    {
        private readonly Mock<IAssetRepository> _repoMock;
        private readonly AssetsController _controller;

        public AssetsControllerTests()
        {
            _repoMock = new Mock<IAssetRepository>();
            _controller = new AssetsController(_repoMock.Object);
        }

        [Fact]
        public async Task SearchCategories_ReturnsOk_WithData()
        {
            var data = new List<AssetCategoryDto> { new AssetCategoryDto { AssetCategoryID = 1L, CategoryName = "Laptop", TenantID = 1 } };
            _repoMock.Setup(r => r.SearchCategoriesAsync(1L, "Laptop", 1, 50)).ReturnsAsync(data);

            var result = await _controller.SearchCategories(1L, "Laptop", 1, 50);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<AssetCategoryDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenDoesNotExist()
        {
            _repoMock.Setup(r => r.GetCategoryByIdAsync(999L, 1L)).ReturnsAsync((AssetCategoryDto?)null);

            var result = await _controller.GetCategoryById(999L, 1L);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsOk_WithId()
        {
            var request = new CreateAssetCategoryRequest { TenantId = 1, CategoryCode = "LAP", CategoryName = "Laptop" };
            _repoMock.Setup(r => r.CreateCategoryAsync(request)).ReturnsAsync(10L);

            var result = await _controller.CreateCategory(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task CreateAsset_ReturnsOk_WithId()
        {
            var request = new CreateAssetRequest { TenantId = 1, AssetCode = "AST-01", AssetTag = "T-01", AssetName = "Laptop Pro" };
            _repoMock.Setup(r => r.CreateAssetAsync(request)).ReturnsAsync(15L);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(15L, response.Data);
        }
    }
}
