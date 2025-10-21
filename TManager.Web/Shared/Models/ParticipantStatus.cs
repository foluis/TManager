namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Participant status constants
    /// </summary>
    public static class ParticipantStatus
    {
        public const string Active = "active";
        public const string Eliminated = "eliminated";
        public const string Withdrawn = "withdrawn";

        /// <summary>
        /// Gets all valid participant statuses
        /// </summary>
        public static readonly string[] AllStatuses = new[]
        {
        Active,
        Eliminated,
        Withdrawn
    };

        /// <summary>
        /// Gets display name for participant status
        /// </summary>
        public static string GetDisplayName(string status) => status switch
        {
            Active => "Active",
            Eliminated => "Eliminated",
            Withdrawn => "Withdrawn",
            _ => "Unknown"
        };

        /// <summary>
        /// Gets badge color for participant status
        /// </summary>
        public static string GetBadgeColor(string status) => status switch
        {
            Active => "badge-success",
            Eliminated => "badge-error",
            Withdrawn => "badge-warning",
            _ => "badge-ghost"
        };

        /// <summary>
        /// Validates if a participant status string is valid
        /// </summary>
        public static bool IsValid(string status) => AllStatuses.Contains(status);
    }
}
