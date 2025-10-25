using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TManager.Web.Shared.Models;

namespace TManager.Web.Features.Auth.Services
{
    /// <summary>
    /// Custom authentication state provider for managing user authentication state
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _authService;
        private UserProfile? _currentUser;

        public CustomAuthStateProvider(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            try
            {
                var user = await GetCurrentUserAsync();

                if (user != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(ClaimTypes.Role, user.GlobalRole),
                    new Claim("Username", user.Username ?? user.Email)
                };

                    identity = new ClaimsIdentity(claims, "Supabase");
                }
            }
            catch (Exception)
            {
                // If there's an error, return unauthenticated state
            }

            var user_principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(user_principal);
        }

        /// <summary>
        /// Gets the current authenticated user
        /// </summary>
        public async Task<UserProfile?> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            _currentUser = await _authService.GetCurrentUserAsync();
            return _currentUser;
        }

        /// <summary>
        /// Notify that the authentication state has changed
        /// </summary>
        public void NotifyAuthenticationStateChanged()
        {
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Mark user as authenticated
        /// </summary>
        public void MarkUserAsAuthenticated(UserProfile user)
        {
            _currentUser = user;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.Role, user.GlobalRole),
            new Claim("Username", user.Username ?? user.Email)
        };

            var identity = new ClaimsIdentity(claims, "Supabase");
            var user_principal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user_principal)));
        }

        /// <summary>
        /// Mark user as logged out
        /// </summary>
        public void MarkUserAsLoggedOut()
        {
            _currentUser = null;

            var identity = new ClaimsIdentity();
            var user_principal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user_principal)));
        }
    }
}
