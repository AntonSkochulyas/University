using University.Data;
using University.Entities;
using University.Repositories.Interfaces;

namespace University.Repositories.Realizations
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(UniversityContext context) : base(context)
        {
        }
    }
}
