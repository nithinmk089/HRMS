using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HRMS.IntegrationTests
{
    public class LearningApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<ILearningRepository> _repoMock = new();

        public LearningApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ILearningRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<ILearningRepository>(_ => _repoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetCourses_ReturnsSuccess()
        {
            var list = new List<CourseDto> { new() { CourseID = 1L, CourseCode = "C001", TenantID = 1 } };
            _repoMock.Setup(r => r.GetCoursesAsync(1L)).ReturnsAsync(list);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.GetAsync("/api/v1/learning/courses?tenantId=1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<List<CourseDto>>>();
            Assert.NotNull(apiResult);
            Assert.True(apiResult.Success);
            Assert.Single(apiResult.Data);
        }

        [Fact]
        public async Task CreateCourse_ReturnsForbidden_WhenNotAdmin()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "USER");

            var response = await client.PostAsJsonAsync("/api/v1/learning/courses", new CreateCourseRequest { TenantID = 1, CourseCode = "C001", CourseName = "Test" });
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateCourse_ReturnsSuccess_WhenAdmin()
        {
            _repoMock.Setup(r => r.CreateCourseAsync(It.IsAny<CreateCourseRequest>())).ReturnsAsync(1L);

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Role", "ADMIN");

            var response = await client.PostAsJsonAsync("/api/v1/learning/courses", new CreateCourseRequest { TenantID = 1, CourseCode = "C001", CourseName = "Test", CourseCategoryID = 1 });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
