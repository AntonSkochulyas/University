using System.ComponentModel.DataAnnotations;

namespace University.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be greater 50 characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be greater 50 characters")]
        public string? LastName { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime? DateOfBirth { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
