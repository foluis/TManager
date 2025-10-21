namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Tournament status constants
    /// </summary>
    public static class TournamentStatus
    {
        public const string Upcoming = "upcoming";
        public const string InProgress = "in_progress";
        public const string Completed = "completed";
        public const string Cancelled = "cancelled";

        /// <summary>
        /// Gets all valid status values
        /// </summary>
        public static readonly string[] AllStatuses = new[]
        {
        Upcoming,
        InProgress,
        Completed,
        Cancelled
    };

        /// <summary>
        /// Gets display name for status
        /// </summary>
        public static string GetDisplayName(string status) => status switch
        {
            Upcoming => "Upcoming",
            InProgress => "In Progress",
            Completed => "Completed",
            Cancelled => "Cancelled",
            _ => "Unknown"
        };

        /// <summary>
        /// Validates if a status string is valid
        /// </summary>
        public static bool IsValid(string status) => AllStatuses.Contains(status);
    }
}
