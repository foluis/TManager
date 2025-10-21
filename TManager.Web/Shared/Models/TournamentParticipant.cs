using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Tournament participant model - matches tournament_participants table
    /// </summary>
    [Table("tournament_participants")]
    public class TournamentParticipant : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("tournament_id")]
        public string TournamentId { get; set; } = string.Empty;

        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = TournamentRole.Participant;

        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }

        [Column("status")]
        public string Status { get; set; } = ParticipantStatus.Active;

        /// <summary>
        /// Checks if participant is tournament admin
        /// </summary>
        public bool IsTournamentAdmin => Role == TournamentRole.TournamentAdmin;

        /// <summary>
        /// Checks if participant is active
        /// </summary>
        public bool IsActive => Status == ParticipantStatus.Active;

        /// <summary>
        /// Checks if participant is eliminated
        /// </summary>
        public bool IsEliminated => Status == ParticipantStatus.Eliminated;

        /// <summary>
        /// Checks if participant has withdrawn
        /// </summary>
        public bool HasWithdrawn => Status == ParticipantStatus.Withdrawn;
    }
}
