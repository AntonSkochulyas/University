using Microsoft.AspNetCore.Mvc;
using Moq;
using University.Controllers;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.XUnitTest.Controllers
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentRepository> _mockRepo;
        private readonly StudentController _studentController;

        public StudentControllerTest()
        {
            _mockRepo = new Mock<IStudentRepository>();
            _studentController = new StudentController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllStudentsTest_ReturnsOkResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(GetTestStudents());

            // Act
            var result = await _studentController.GetStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Student>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetStudent_ReturnsNotFoundResult()
        {
            // Arrange
            var testId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId))
                     .ReturnsAsync((Student)null);

            // Act
            var result = await _studentController.GetStudentById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostStudent_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newStudent = new Student { StudentId = 3, FirstName = "Anton", Email = "anton.skochulyas@gmail.com" };

            // Act
            var result = await _studentController.CreateStudent(newStudent);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Student>(createdAtActionResult.Value);
            Assert.Equal(newStudent.StudentId, returnValue.StudentId);
        }

        [Fact]
        public async Task PutStudent_ReturnsNoContentResult()
        {
            // Arrange
            var testId = 1;
            var student = GetTestStudents()[0];
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(student);

            // Act
            var result = await _studentController.UpdateStudent(testId, student);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutStudent_ReturnsBadRequestResult()
        {
            // Arrange
            var testId = 1;
            var student = new Student { StudentId = 2, FirstName = "Anton", Email = "anton.skochulyas@gmail.com" };

            // Act
            var result = await _studentController.UpdateStudent(testId, student);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNoContentResult()
        {
            // Arrange
            var testId = 1;
            var student = GetTestStudents()[0];
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync(student);

            // Act
            var result = await _studentController.DeleteStudent(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNotFoundResult()
        {
            // Arrange
            var testId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(testId)).ReturnsAsync((Student)null);

            // Act
            var result = await _studentController.DeleteStudent(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        private List<Student> GetTestStudents()
        {
            return new List<Student>
            {
                new Student { StudentId = 1, FirstName = "Anton", Email = "anton.skochulyas@gmail.com" },
                new Student { StudentId = 2, FirstName = "Olga", Email = "olga.melnychuk@gmail.com" }
            };
        }
    }
}
