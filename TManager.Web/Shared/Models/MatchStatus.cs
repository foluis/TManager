namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Match status constants
    /// </summary>
    public static class MatchStatus
    {
        public const string Pending = "pending";
        public const string InProgress = "in_progress";
        public const string Completed = "completed";
        public const string Cancelled = "cancelled";

        /// <summary>
        /// Gets all valid match statuses
        /// </summary>
        public static readonly string[] AllStatuses = new[]
        {
        Pending,
        InProgress,
        Completed,
        Cancelled
    };

        /// <summary>
        /// Gets display name for match status
        /// </summary>
        public static string GetDisplayName(string status) => status switch
        {
            Pending => "Pending",
            InProgress => "In Progress",
            Completed => "Completed",
            Cancelled => "Cancelled",
            _ => "Unknown"
        };

        /// <summary>
        /// Gets badge color for match status
        /// </summary>
        public static string GetBadgeColor(string status) => status switch
        {
            Pending => "badge-info",
            InProgress => "badge-warning",
            Completed => "badge-success",
            Cancelled => "badge-error",
            _ => "badge-ghost"
        };

        /// <summary>
        /// Validates if a match status string is valid
        /// </summary>
        public static bool IsValid(string status) => AllStatuses.Contains(status);
    }
}
