using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ITProductECommerce.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage ="Username is required")]
        [MaxLength(50, ErrorMessage ="Maximum 50 chars")]
        public string UserId { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 chars")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 chars")]
        public string LastName { get; set; }

        public bool Gender { get; set; } = true;

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DoB { get; set; }

        [Display(Name = "Address")]
        [MaxLength(100, ErrorMessage = "Maximum 100 chars")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(24, ErrorMessage = "Maximum 24 chars")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage ="Phone Number Vietnamese format error")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Email format error")]
        public string Email { get; set; }

        [Display(Name = "Image")]
        public string? AvatarURL { get; set; } = "";
    }
}
