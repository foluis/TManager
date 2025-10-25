using System.ComponentModel.DataAnnotations;

namespace TManager.Web.Features.Auth.Login
{
    /// <summary>
    /// Command for user login
    /// </summary>
    public class LoginCommand
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
