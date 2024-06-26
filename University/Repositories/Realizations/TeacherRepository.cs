using University.Data;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.Repositories.Realizations
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(UniversityContext context) : base(context)
        {
        }
    }
}
