using Supabase.Gotrue.Exceptions;
using TManager.Web.Infrastructure.Supabase;
using TManager.Web.Shared.Models;

namespace TManager.Web.Features.Auth.Services
{
    /// <summary>
    /// Implementation of authentication service using Supabase
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly SupabaseClientWrapper _supabase;

        public AuthService(SupabaseClientWrapper supabase)
        {
            _supabase = supabase;
        }

        public async Task<AuthResult> SignUpAsync(string email, string password)
        {
            try
            {
                await _supabase.InitializeAsync();

                // Sign up with Supabase Auth
                var session = await _supabase.Client.Auth.SignUp(email, password);

                if (session?.User == null)
                {
                    return AuthResult.Failed("Failed to create account. Please try again.");
                }

                // Wait a moment for the trigger to create the profile
                await Task.Delay(500);

                // Fetch the created user profile
                var profile = await GetUserProfileAsync(session.User.Id);

                if (profile == null)
                {
                    return AuthResult.Failed("Account created but profile not found. Please try logging in.");
                }

                return AuthResult.Successful(profile);
            }
            catch (GotrueException ex)
            {
                return AuthResult.Failed(GetFriendlyErrorMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return AuthResult.Failed($"An error occurred: {ex.Message}");
            }
        }

        public async Task<AuthResult> SignInAsync(string email, string password)
        {
            try
            {
                await _supabase.InitializeAsync();

                // Sign in with Supabase Auth
                var session = await _supabase.Client.Auth.SignIn(email, password);

                if (session?.User == null)
                {
                    return AuthResult.Failed("Invalid email or password.");
                }

                // Fetch user profile
                var profile = await GetUserProfileAsync(session.User.Id);

                if (profile == null)
                {
                    return AuthResult.Failed("User profile not found.");
                }

                return AuthResult.Successful(profile);
            }
            catch (GotrueException ex)
            {
                return AuthResult.Failed(GetFriendlyErrorMessage(ex.Message));
            }
            catch (Exception ex)
            {
                return AuthResult.Failed($"An error occurred: {ex.Message}");
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                await _supabase.InitializeAsync();
                await _supabase.Client.Auth.SignOut();
            }
            catch (Exception)
            {
                // Silently fail - user is being logged out anyway
            }
        }

        public async Task<UserProfile?> GetCurrentUserAsync()
        {
            try
            {
                await _supabase.InitializeAsync();

                var user = _supabase.Client.Auth.CurrentUser;
                if (user == null) return null;

                return await GetUserProfileAsync(user.Id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                await _supabase.InitializeAsync();
                return _supabase.Client.Auth.CurrentUser != null;
            }
            catch
            {
                return false;
            }
        }

        private async Task<UserProfile?> GetUserProfileAsync(string userId)
        {
            try
            {
                var response = await _supabase.Client
                    .From<UserProfile>()
                    .Where(x => x.Id == userId)
                    .Single();

                return response;
            }
            catch
            {
                return null;
            }
        }

        private string GetFriendlyErrorMessage(string errorMessage)
        {
            // Convert Supabase error messages to user-friendly messages
            if (errorMessage.Contains("already registered"))
                return "This email is already registered. Please sign in instead.";

            if (errorMessage.Contains("Invalid login credentials"))
                return "Invalid email or password. Please try again.";

            if (errorMessage.Contains("Email not confirmed"))
                return "Please check your email and confirm your account before signing in.";

            if (errorMessage.Contains("password"))
                return "Password must be at least 6 characters long.";

            return errorMessage;
        }
    }
}
