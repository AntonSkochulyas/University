using Microsoft.AspNetCore.Mvc;
using Moq;
using University.Controllers;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.XUnitTest.Controllers
{
    public class TeacherControllerTests
    {
        private readonly Mock<ITeacherRepository> _mockRepo;
        private readonly TeacherController _controller;

        public TeacherControllerTests()
        {
            _mockRepo = new Mock<ITeacherRepository>();
            _controller = new TeacherController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetTeachers_ReturnsOkResult_WithListOfTeachers()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(GetTestTeachers());

            // Act
            var result = await _controller.GetTeachers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Teacher>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetTeacher_ReturnsNotFoundResult()
        {
            // Arrange
            var testId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId))
                     .ReturnsAsync((Teacher)null);

            // Act
            var result = await _controller.GetTeacherById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostTeacher_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newTeacher = new Teacher { TeacherId = 3, FirstName = "Olga", Email = "olga.melnychuk@gmail.com" };

            // Act
            var result = await _controller.CreateTeacher(newTeacher);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Teacher>(createdAtActionResult.Value);
            Assert.Equal(newTeacher.TeacherId, returnValue.TeacherId);
        }

        [Fact]
        public async Task PutTeacher_ReturnsNoContentResult()
        {
            // Arrange
            var testId = 1;
            var teacher = GetTestTeachers()[0];
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(teacher);

            // Act
            var result = await _controller.UpdateTeacher(testId, teacher);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutTeacher_ReturnsBadRequestResult()
        {
            // Arrange
            var testId = 1;
            var teacher = new Teacher { TeacherId = 2, FirstName = "Olga", Email = "olga.melnychuk@gmail.com" };

            // Act
            var result = await _controller.UpdateTeacher(testId, teacher);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTeacher_ReturnsNoContentResult()
        {
            // Arrange
            var testId = 1;
            var teacher = GetTestTeachers()[0];
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(teacher);

            // Act
            var result = await _controller.DeleteTeacher(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTeacher_ReturnsNotFoundResult()
        {
            // Arrange
            var testId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync((Teacher)null);

            // Act
            var result = await _controller.DeleteTeacher(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        private List<Teacher> GetTestTeachers()
        {
            return new List<Teacher>
            {
                new Teacher { TeacherId = 1, FirstName = "Anton", Email = "anton.skochulyas@gmail.com" },
                new Teacher { TeacherId = 2, FirstName = "Olga", Email = "olga.melnychuk@gmail.com" }
            };
        }
    }
}
