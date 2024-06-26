using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseRepository _courseRepository;

        public CourseController(ILogger<CourseController> logger, ICourseRepository courseRepository)
        {
            _logger = logger;
            _courseRepository = courseRepository;
        }

        // GET: api/courses
        [HttpGet]
        [SwaggerOperation(Summary = "Get all courses", Description = "Returns a list of all courses.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            _logger.LogInformation($"Fetching all courses");

            try
            {
                return Ok(await _courseRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all courses");

                throw new Exception("Can't get all courses", ex);
            }
            
        }

        // GET: api/courses/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get course by ID", Description = "Returns a single course by their unique ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            _logger.LogInformation($"Fetching course with ID: {id}");

            try
            {
                var course = await _courseRepository.GetByIdAsync(id);

                if (course == null)
                {
                    _logger.LogWarning($"Course with ID {id} not found");

                    return NotFound();
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching course with ID {id}");

                throw new Exception("Can't get course with this Id", ex);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new course", Description = "Creates a new course.")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Course))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid course model received");

                return BadRequest(ModelState);
            }

            try
            {
                await _courseRepository.AddAsync(course);
                await _courseRepository.SaveAsync();

                _logger.LogInformation($"Course with ID {course.CourseId} created");

                return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new course");

                throw new Exception("Can't create new course", ex);
            }
        }

        // PUT: api/courses/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing course", Description = "Updates an existing course by their ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                _logger.LogWarning($"Mismatch between route ID and course ID: {id} != {course.CourseId}");

                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid course model received for update");

                return BadRequest(ModelState);
            }

            try
            {
                _courseRepository.Update(course);
                await _courseRepository.SaveAsync();

                _logger.LogInformation($"Course with ID {course.CourseId} updated");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating course with ID {course.CourseId}");

                throw new Exception("Can't update course", ex);
            }
        }

        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a course", Description = "Deletes a course by their ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            _logger.LogInformation($"Attempting to delete course with ID: {id}");

            try
            {
                var course = await _courseRepository.GetByIdAsync(id);

                if (course == null)
                {
                    _logger.LogWarning($"Course with ID {id} not found");

                    return NotFound();
                }

                _courseRepository.Delete(course);
                await _courseRepository.SaveAsync();

                _logger.LogInformation($"Course with ID {id} deleted");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting course with ID {id}");

                throw new Exception("Can't delete course", ex);
            }
        }
    }
}
