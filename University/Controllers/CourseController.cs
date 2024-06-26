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
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // GET: api/courses
        [HttpGet]
        [SwaggerOperation(Summary = "Get all courses", Description = "Returns a list of all courses.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try
            {
                return Ok(await _courseRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
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
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);

                if (course == null)
                    return NotFound();

                return Ok(course);
            }
            catch (Exception ex)
            {
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
                return BadRequest(ModelState);

            try
            {
                await _courseRepository.AddAsync(course);
                await _courseRepository.SaveAsync();

                return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
            }
            catch (Exception ex)
            {
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
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _courseRepository.Update(course);
                await _courseRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
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
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);

                if (course == null)
                    return NotFound();

                _courseRepository.Delete(course);
                await _courseRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't delete course", ex);
            }
        }
    }
}
