using University.Data;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.Repositories.Realizations
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(UniversityContext context) : base(context)
        {
        }
    }
}
