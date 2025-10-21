using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Tournament model - matches tournaments table in database
    /// </summary>
    [Table("tournaments")]
    public class Tournament : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; } = string.Empty;

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("status")]
        public string Status { get; set; } = TournamentStatus.Upcoming;

        [Column("max_participants")]
        public int? MaxParticipants { get; set; }

        [Column("tournament_type")]
        public string TournamentType { get; set; } = TournamentTypes.SingleElimination;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Checks if tournament is upcoming
        /// </summary>
        public bool IsUpcoming => Status == TournamentStatus.Upcoming;

        /// <summary>
        /// Checks if tournament is in progress
        /// </summary>
        public bool IsInProgress => Status == TournamentStatus.InProgress;

        /// <summary>
        /// Checks if tournament is completed
        /// </summary>
        public bool IsCompleted => Status == TournamentStatus.Completed;

        /// <summary>
        /// Checks if tournament is cancelled
        /// </summary>
        public bool IsCancelled => Status == TournamentStatus.Cancelled;

        /// <summary>
        /// Gets formatted date range
        /// </summary>
        public string DateRange
        {
            get
            {
                if (!StartDate.HasValue) return "Date TBD";

                var start = StartDate.Value.ToString("MMM dd, yyyy");
                if (!EndDate.HasValue) return start;

                var end = EndDate.Value.ToString("MMM dd, yyyy");
                return $"{start} - {end}";
            }
        }

        /// <summary>
        /// Gets status badge color for UI
        /// </summary>
        public string StatusBadgeColor => Status switch
        {
            TournamentStatus.Upcoming => "badge-info",
            TournamentStatus.InProgress => "badge-warning",
            TournamentStatus.Completed => "badge-success",
            TournamentStatus.Cancelled => "badge-error",
            _ => "badge-ghost"
        };
    }
}
