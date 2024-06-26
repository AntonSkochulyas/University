using System.ComponentModel.DataAnnotations;

namespace University.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Course Name is required")]
        [StringLength(50, ErrorMessage = "Course Name cannot be greater 50 characters")]
        public string? CourseName { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
