using ITProductECommerce.Data;
using System.ComponentModel.DataAnnotations;

namespace ITProductECommerce.ViewModels
{
    public class StaffViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        public string StaffId { get; set; } = "";

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 chars")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 chars")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Email format error")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public bool Gender { get; set; }
        public DateTime? DoB { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Avatar { get; set; } = "";
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public IEnumerable<int> Pages { get; set; }
        public IEnumerable<User> Staffs { get; set; }
        public string? Search { get; set; }
    }
}
