﻿using System.ComponentModel.DataAnnotations;

namespace ITProductECommerce.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Maximum 20 chars")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Note { get; set; }
    }
}