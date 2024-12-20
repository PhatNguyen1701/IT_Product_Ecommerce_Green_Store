﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ITProductECommerce.ViewModels
{
    public class UserProfileVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        public string UserId { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Customer's name is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 chars")]
        public string FirstName { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Customer's name is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 chars")]
        public string LastName { get; set; }

        public bool Gender { get; set; } = true;

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DoB { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60, ErrorMessage = "Maximum 60 chars")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(24, ErrorMessage = "Maximum 24 chars")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage = "Phone Number Vietnamese format error")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email format error")]
        public string Email { get; set; }

        [Display(Name = "Image")]
        public string? AvatarURL { get; set; }
    }
}
