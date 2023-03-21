﻿using System.ComponentModel.DataAnnotations;

namespace ObjectDetectionAPI.Models.Authentication.Login
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
