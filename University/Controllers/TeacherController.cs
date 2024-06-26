using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ILogger<TeacherController> logger, ITeacherRepository teacherRepository)
        {
            _logger = logger;
            _teacherRepository = teacherRepository;
        }

        // GET: api/teachers
        [HttpGet]
        [SwaggerOperation(Summary = "Get all teachers", Description = "Returns a list of all teachers.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teacher>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            _logger.LogInformation($"Fetching all teachers");

            try
            {
                return Ok(await _teacherRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all teachers");

                throw new Exception("Can't get all teachers", ex);
            }
        }

        // GET: api/teachers/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get teacher by ID", Description = "Returns a single teacher by their unique ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Teacher>> GetTeacherById(int id)
        {
            _logger.LogInformation($"Fetching teacher with ID: {id}");

            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                {
                    _logger.LogWarning($"Teacher with ID {id} not found");

                    return NotFound();
                }
                return Ok(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching teacher with ID {id}");

                throw new Exception("Can't get teacher with this Id", ex);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new teacher", Description = "Creates a new teacher.")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Teacher))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid teacher model received");

                return BadRequest(ModelState);
            }

            try
            {
                await _teacherRepository.AddAsync(teacher);
                await _teacherRepository.SaveAsync();

                _logger.LogInformation($"Teacher with ID {teacher.TeacherId} created");

                return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new teacher");

                throw new Exception("Can't create new teacher", ex);
            }
        }

        // PUT: api/teachers/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing teacher", Description = "Updates an existing teacher by their ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTeacher(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                _logger.LogWarning($"Mismatch between route ID and teacher ID: {id} != {teacher.TeacherId}");

                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid teacher model received for update");

                return BadRequest(ModelState);
            }

            try
            {
                _teacherRepository.Update(teacher);
                await _teacherRepository.SaveAsync();

                _logger.LogInformation($"Teacher with ID {teacher.TeacherId} updated");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating teacher with ID {teacher.TeacherId}");

                throw new Exception("Can't update teacher", ex);
            }
        }

        // DELETE: api/teachers/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a teacher", Description = "Deletes a teacher by their ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            _logger.LogInformation($"Attempting to delete teacher with ID: {id}");

            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                {
                    _logger.LogWarning($"Teacher with ID {id} not found");

                    return NotFound();
                }

                _teacherRepository.Delete(teacher);
                await _teacherRepository.SaveAsync();

                _logger.LogInformation($"Teacher with ID {id} deleted");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting teacher with ID {id}");

                throw new Exception("Can't delete teacher", ex);
            }
        }
    }
}
