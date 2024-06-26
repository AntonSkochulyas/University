using University.Entities;

namespace University.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UniversityContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Look for any students
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            // Add random data
            var random = new Random();

            // Add some Teachers
            var teachers = new Teacher[]
            {
                new Teacher { FirstName = "Anton", LastName = "Skochulyas", Email = "anton.skochulyas@gmail.com", HireDate = DateTime.Now.AddYears(-10) },
                new Teacher { FirstName = "Olga", LastName = "Melnychuk", Email = "olga.melnychuk@gmail.com", HireDate = DateTime.Now.AddYears(-5) }
            };

            foreach (var teacher in teachers)
            {
                context.Teachers.Add(teacher);
            }

            context.SaveChanges();

            // Add Courses
            var courses = new Course[]
            {
                new Course { CourseName = "Math", TeacherId = teachers[0].TeacherId },
                new Course { CourseName = "English",  TeacherId = teachers[1].TeacherId }
            };

            foreach (var course in courses)
            {
                context.Courses.Add(course);
            }

            context.SaveChanges();

            // Add Students
            var students = new Student[10];
            for (int i = 0; i < 10; i++)
            {
                students[i] = new Student
                {
                    FirstName = "Student" + i,
                    LastName = "LastName" + i,
                    Email = $"student{i}@example.com",
                    DateOfBirth = DateTime.Now.AddYears(-20).AddDays(random.Next(-1000, 1000))
                };

                context.Students.Add(students[i]);
            }

            context.SaveChanges();

            // Add Student-Course relationships
            foreach (var student in students)
            {
                var selectedCourses = context.Courses.OrderBy(c => random.Next()).Take(random.Next(0, 1)).ToList();
                foreach (var course in selectedCourses)
                {
                    student.Courses.Add(course);
                }
            }

            context.SaveChanges();
        }
    
    }
}
