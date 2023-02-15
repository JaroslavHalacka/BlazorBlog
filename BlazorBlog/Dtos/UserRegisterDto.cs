using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Dtos
{
    public class UserRegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The password do not match")]
        public string ConfirmPasword { get; set; } = string.Empty;
    }
}
