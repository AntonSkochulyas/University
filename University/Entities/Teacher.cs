using System.ComponentModel.DataAnnotations;

namespace University.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be greater 50 characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be greater 50 characters")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Hire Date is required")]
        public DateTime HireDate { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
