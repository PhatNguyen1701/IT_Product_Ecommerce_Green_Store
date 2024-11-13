using System.ComponentModel.DataAnnotations;

namespace ITProductECommerce.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
