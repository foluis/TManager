using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Match model - matches matches table in database
    /// </summary>
    [Table("matches")]
    public class Match : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("tournament_id")]
        public string TournamentId { get; set; } = string.Empty;

        [Column("round_number")]
        public int RoundNumber { get; set; }

        [Column("match_number")]
        public int MatchNumber { get; set; }

        [Column("player1_id")]
        public string? Player1Id { get; set; }

        [Column("player2_id")]
        public string? Player2Id { get; set; }

        [Column("winner_id")]
        public string? WinnerId { get; set; }

        [Column("scheduled_time")]
        public DateTime? ScheduledTime { get; set; }

        [Column("status")]
        public string Status { get; set; } = MatchStatus.Pending;

        [Column("score_player1")]
        public int? ScorePlayer1 { get; set; }

        [Column("score_player2")]
        public int? ScorePlayer2 { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Checks if match is pending
        /// </summary>
        public bool IsPending => Status == MatchStatus.Pending;

        /// <summary>
        /// Checks if match is in progress
        /// </summary>
        public bool IsInProgress => Status == MatchStatus.InProgress;

        /// <summary>
        /// Checks if match is completed
        /// </summary>
        public bool IsCompleted => Status == MatchStatus.Completed;

        /// <summary>
        /// Checks if match is cancelled
        /// </summary>
        public bool IsCancelled => Status == MatchStatus.Cancelled;

        /// <summary>
        /// Gets formatted score display
        /// </summary>
        public string ScoreDisplay
        {
            get
            {
                if (!ScorePlayer1.HasValue || !ScorePlayer2.HasValue)
                    return "No score";

                return $"{ScorePlayer1} - {ScorePlayer2}";
            }
        }

        /// <summary>
        /// Gets match title
        /// </summary>
        public string MatchTitle => $"Round {RoundNumber} - Match {MatchNumber}";
    }
}
