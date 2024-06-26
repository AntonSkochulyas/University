using Microsoft.AspNetCore.Mvc;
using Moq;
using University.Controllers;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.XUnitTest.Controllers
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseRepository> _mockRepo;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            _mockRepo = new Mock<ICourseRepository>();
            _controller = new CourseController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCourses_ReturnsOkResult_WithListOfCourses()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(GetTestCourses());

            // Act
            var result = await _controller.GetCourses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Course>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCourse_ReturnsNotFoundResult()
        {
            // Arrange
            var testId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId))
                     .ReturnsAsync((Course)null);

            // Act
            var result = await _controller.GetCourseById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostCourse_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newCourse = new Course { CourseId = 3, CourseName = "Math", TeacherId = 1 };

            // Act
            var result = await _controller.CreateCourse(newCourse);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Course>(createdAtActionResult.Value);
            Assert.Equal(newCourse.CourseId, returnValue.CourseId);
        }

        [Fact]
        public async Task PutCourse_ReturnsNoContentResult()
        {
            // Arrange
            var testId = 1;
            var course = GetTestCourses()[0];
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(course);

            // Act
            var result = await _controller.UpdateCourse(testId, course);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCourse_ReturnsBadRequestResult()
        {
            // Arrange
            var testId = 1;
            var course = new Course { CourseId = 2, CourseName = "Physics", TeacherId = 2 };

            // Act
            var result = await _controller.UpdateCourse(testId, course);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCourse_ReturnsNoContentResult()
        {
            // Arrange
            var testId = 1;
            var course = GetTestCourses()[0];
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(course);

            // Act
            var result = await _controller.DeleteCourse(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCourse_ReturnsNotFoundResult()
        {
            // Arrange
            var testId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync((Course)null);

            // Act
            var result = await _controller.DeleteCourse(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        private List<Course> GetTestCourses()
        {
            return new List<Course>
            {
                new Course { CourseId = 1, CourseName = "Math", TeacherId = 1 },
                new Course { CourseId = 2, CourseName = "Physics", TeacherId = 2 }
            };
        }
    }
}
