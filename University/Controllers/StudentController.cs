using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: api/students
        [HttpGet]
        [SwaggerOperation(Summary = "Get all students", Description = "Returns a list of all students.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                return Ok(await _studentRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                throw new Exception("Can't get all students", ex);
            }
            
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get student by ID", Description = "Returns a single student by their unique ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                    return NotFound();

                return Ok(student);
            }
            catch (Exception ex)
            {
                throw new Exception("Can't get student with this Id", ex);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new student", Description = "Creates a new student.")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveAsync();

                return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                throw new Exception("Can't create new student", ex);
            }
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing student", Description = "Updates an existing student by their ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _studentRepository.Update(student);
                await _studentRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't update student", ex);
            }
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a student", Description = "Deletes a student by their ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                    return NotFound();

                _studentRepository.Delete(student);
                await _studentRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't delete student", ex);
            }
        }
    }
}
