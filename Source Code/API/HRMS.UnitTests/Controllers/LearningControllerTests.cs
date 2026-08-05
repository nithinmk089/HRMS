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
    public class LearningControllerTests
    {
        private readonly Mock<ILearningRepository> _repoMock = new();
        private readonly LearningController _controller;

        public LearningControllerTests()
        {
            _controller = new LearningController(_repoMock.Object);
        }

        [Fact]
        public async Task GetCourses_ReturnsOk()
        {
            var list = new List<CourseDto> { new() { CourseID = 1L, CourseCode = "C001" } };
            _repoMock.Setup(r => r.GetCoursesAsync(1L)).ReturnsAsync(list);
            var result = await _controller.GetCourses(1L);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<CourseDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task CreateCourse_ReturnsOk()
        {
            var req = new CreateCourseRequest { TenantID = 1, CourseCode = "C001", CourseName = "Angular Basics", CourseCategoryID = 1 };
            _repoMock.Setup(r => r.CreateCourseAsync(req)).ReturnsAsync(1L);
            var result = await _controller.CreateCourse(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1L, response.Data);
        }

        [Fact]
        public async Task GetEnrollments_ReturnsOk()
        {
            _repoMock.Setup(r => r.GetEnrollmentsAsync(1L, null)).ReturnsAsync(new List<EnrollmentDto>());
            var result = await _controller.GetEnrollments(1L, null);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task SubmitAssessmentResult_ReturnsOk()
        {
            var req = new SubmitAssessmentResultRequest { TenantID = 1, AssessmentID = 1, EmployeeID = 5, Score = 85 };
            _repoMock.Setup(r => r.SubmitAssessmentResultAsync(req)).ReturnsAsync(10L);
            var result = await _controller.SubmitAssessmentResult(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task RecordComplianceAck_ReturnsOk()
        {
            var req = new RecordComplianceAckRequest { TenantID = 1, EmployeeID = 5, ComplianceTrainingID = 1 };
            _repoMock.Setup(r => r.RecordComplianceAckAsync(req)).ReturnsAsync(20L);
            var result = await _controller.RecordComplianceAck(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(20L, response.Data);
        }
    }
}
