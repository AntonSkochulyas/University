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
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        // GET: api/teachers
        [HttpGet]
        [SwaggerOperation(Summary = "Get all teachers", Description = "Returns a list of all teachers.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teacher>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            try
            {
                return Ok(await _teacherRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
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
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                    return NotFound();

                return Ok(teacher);
            }
            catch (Exception ex)
            {
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
                return BadRequest(ModelState);

            try
            {
                await _teacherRepository.AddAsync(teacher);
                await _teacherRepository.SaveAsync();

                return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
            }
            catch (Exception ex)
            {
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
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _teacherRepository.Update(teacher);
                await _teacherRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
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
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(id);

                if (teacher == null)
                    return NotFound();

                _teacherRepository.Delete(teacher);
                await _teacherRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't delete teacher", ex);
            }
        }
    }
}
