namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Tournament-specific role constants
    /// </summary>
    public static class TournamentRole
    {
        public const string TournamentAdmin = "tournament_admin";
        public const string Participant = "participant";

        /// <summary>
        /// Gets all valid tournament roles
        /// </summary>
        public static readonly string[] AllRoles = new[]
        {
        TournamentAdmin,
        Participant
    };

        /// <summary>
        /// Gets display name for tournament role
        /// </summary>
        public static string GetDisplayName(string role) => role switch
        {
            TournamentAdmin => "Tournament Admin",
            Participant => "Participant",
            _ => "Unknown"
        };

        /// <summary>
        /// Validates if a tournament role string is valid
        /// </summary>
        public static bool IsValid(string role) => AllRoles.Contains(role);
    }
}
