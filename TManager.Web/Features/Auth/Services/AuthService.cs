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

                Console.WriteLine($"[SignUp] Starting signup for: {email}");

                // Sign up with Supabase Auth
                var session = await _supabase.Client.Auth.SignUp(email, password);

                Console.WriteLine($"[SignUp] Session created: {session != null}");
                Console.WriteLine($"[SignUp] User created: {session?.User != null}");
                Console.WriteLine($"[SignUp] User ID: {session?.User?.Id}");

                if (session?.User == null)
                {
                    Console.WriteLine("[SignUp] ERROR: Failed to create auth user");
                    return AuthResult.Failed("Failed to create account. Please try again.");
                }

                // Wait for the trigger to create the profile with retry logic
                UserProfile? profile = null;
                int maxRetries = 10;
                int retryDelay = 300;

                Console.WriteLine("[SignUp] Waiting for profile creation...");

                for (int i = 0; i < maxRetries; i++)
                {
                    await Task.Delay(retryDelay);
                    profile = await GetUserProfileAsync(session.User.Id);

                    Console.WriteLine($"[SignUp] Retry {i + 1}/{maxRetries}: Profile found = {profile != null}");

                    if (profile != null)
                        break;
                }

                if (profile == null)
                {
                    Console.WriteLine("[SignUp] ERROR: Profile not found after retries");
                    return AuthResult.Failed("Account created successfully! Please sign in to continue.");
                }

                Console.WriteLine($"[SignUp] SUCCESS: Profile found for {profile.Email}");
                return AuthResult.Successful(profile);
            }
            catch (GotrueException ex)
            {
                Console.WriteLine($"[SignUp] GotrueException: {ex.Message}");
                return AuthResult.Failed(GetFriendlyErrorMessage(ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignUp] Exception: {ex.Message}");
                Console.WriteLine($"[SignUp] Stack: {ex.StackTrace}");
                return AuthResult.Failed($"An error occurred: {ex.Message}");
            }
        }

        public async Task<AuthResult> SignInAsync(string email, string password)
        {
            try
            {
                await _supabase.InitializeAsync();

                Console.WriteLine($"[SignIn] Starting signin for: {email}");

                // Sign in with Supabase Auth
                var session = await _supabase.Client.Auth.SignIn(email, password);

                Console.WriteLine($"[SignIn] Session created: {session != null}");
                Console.WriteLine($"[SignIn] User authenticated: {session?.User != null}");
                Console.WriteLine($"[SignIn] User ID: {session?.User?.Id}");
                Console.WriteLine($"[SignIn] Access Token exists: {!string.IsNullOrEmpty(session?.AccessToken)}");

                if (session?.User == null)
                {
                    Console.WriteLine("[SignIn] ERROR: Invalid credentials");
                    return AuthResult.Failed("Invalid email or password.");
                }

                // IMPORTANT: The session is now active, auth.uid() should work
                // Wait a moment for the session to be fully established
                await Task.Delay(500);

                // Try multiple times to fetch profile
                UserProfile? profile = null;
                int maxRetries = 5;

                Console.WriteLine("[SignIn] Fetching user profile...");

                for (int i = 0; i < maxRetries; i++)
                {
                    profile = await GetUserProfileAsync(session.User.Id);

                    Console.WriteLine($"[SignIn] Retry {i + 1}/{maxRetries}: Profile found = {profile != null}");

                    if (profile != null)
                        break;

                    await Task.Delay(300);
                }

                if (profile == null)
                {
                    Console.WriteLine("[SignIn] ERROR: Profile not found");
                    Console.WriteLine($"[SignIn] Attempting direct query by email...");

                    // Try fetching by email as a fallback
                    profile = await GetUserProfileByEmailAsync(email);

                    if (profile != null)
                    {
                        Console.WriteLine($"[SignIn] SUCCESS: Profile found by email");
                    }
                }

                if (profile == null)
                {
                    Console.WriteLine("[SignIn] ERROR: All profile fetch attempts failed");
                    return AuthResult.Failed("User profile not found.");
                }

                Console.WriteLine($"[SignIn] SUCCESS: Login complete for {profile.Email}");
                return AuthResult.Successful(profile);
            }
            catch (GotrueException ex)
            {
                Console.WriteLine($"[SignIn] GotrueException: {ex.Message}");
                return AuthResult.Failed(GetFriendlyErrorMessage(ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignIn] Exception: {ex.Message}");
                Console.WriteLine($"[SignIn] Stack: {ex.StackTrace}");
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
                Console.WriteLine($"[GetProfile] Attempting to fetch profile for user ID: {userId}");

                // Method 1: Try with Select and Filter
                var response = await _supabase.Client
                    .From<UserProfile>()
                    .Select("*")
                    .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, userId)
                    .Limit(1)
                    .Get();

                Console.WriteLine($"[GetProfile] Method 1 response: Models count = {response?.Models?.Count ?? 0}");

                if (response?.Models != null && response.Models.Count > 0)
                {
                    Console.WriteLine($"[GetProfile] SUCCESS: Profile found via Filter");
                    return response.Models[0];
                }

                // Method 2: Try with Where clause
                var response2 = await _supabase.Client
                    .From<UserProfile>()
                    .Select("*")
                    .Where(x => x.Id == userId)
                    .Get();

                Console.WriteLine($"[GetProfile] Method 2 response: Models count = {response2?.Models?.Count ?? 0}");

                if (response2?.Models != null && response2.Models.Count > 0)
                {
                    Console.WriteLine($"[GetProfile] SUCCESS: Profile found via Where");
                    return response2.Models[0];
                }

                Console.WriteLine($"[GetProfile] FAILED: No profile found for user ID: {userId}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetProfile] ERROR: {ex.Message}");
                return null;
            }
        }

        private async Task<UserProfile?> GetUserProfileByEmailAsync(string email)
        {
            try
            {
                Console.WriteLine($"[GetProfileByEmail] Attempting to fetch profile for email: {email}");

                var response = await _supabase.Client
                    .From<UserProfile>()
                    .Select("*")
                    .Filter("email", Supabase.Postgrest.Constants.Operator.Equals, email)
                    .Limit(1)
                    .Get();

                Console.WriteLine($"[GetProfileByEmail] Response: Models count = {response?.Models?.Count ?? 0}");

                if (response?.Models != null && response.Models.Count > 0)
                {
                    Console.WriteLine($"[GetProfileByEmail] SUCCESS: Profile found");
                    return response.Models[0];
                }

                Console.WriteLine($"[GetProfileByEmail] FAILED: No profile found for email: {email}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetProfileByEmail] ERROR: {ex.Message}");
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