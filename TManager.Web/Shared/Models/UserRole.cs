namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Global application roles for users
    /// </summary>
    public static class UserRole
    {
        public const string RegularUser = "regular_user";
        public const string Organizer = "organizer";
        public const string Admin = "admin";

        /// <summary>
        /// Gets all valid role values
        /// </summary>
        public static readonly string[] AllRoles = new[]
        {
        RegularUser,
        Organizer,
        Admin
    };

        /// <summary>
        /// Gets display name for a role
        /// </summary>
        public static string GetDisplayName(string role) => role switch
        {
            RegularUser => "Regular User",
            Organizer => "Organizer",
            Admin => "Administrator",
            _ => "Unknown"
        };

        /// <summary>
        /// Validates if a role string is valid
        /// </summary>
        public static bool IsValid(string role) => AllRoles.Contains(role);
    }
}
