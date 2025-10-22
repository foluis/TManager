using TManager.Web.Shared.Models;

namespace TManager.Web.Features.Auth.Services
{
    /// <summary>
    /// Interface for authentication operations
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Sign up a new user with email and password
        /// </summary>
        Task<AuthResult> SignUpAsync(string email, string password);

        /// <summary>
        /// Sign in an existing user with email and password
        /// </summary>
        Task<AuthResult> SignInAsync(string email, string password);

        /// <summary>
        /// Sign out the current user
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        /// Get the current authenticated user profile
        /// </summary>
        Task<UserProfile?> GetCurrentUserAsync();

        /// <summary>
        /// Check if user is authenticated
        /// </summary>
        Task<bool> IsAuthenticatedAsync();
    }

    /// <summary>
    /// Result of authentication operations
    /// </summary>
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public UserProfile? User { get; set; }

        public static AuthResult Successful(UserProfile user) => new()
        {
            Success = true,
            User = user
        };

        public static AuthResult Failed(string errorMessage) => new()
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
