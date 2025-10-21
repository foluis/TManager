using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TManager.Web.Shared.Models
{  
    /// <summary>
    /// User profile model - matches users_profile table in database
    /// </summary>
    [Table("users_profile")]
    public class UserProfile : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("username")]
        public string? Username { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("global_role")]
        public string GlobalRole { get; set; } = UserRole.RegularUser;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets the display name (full name if available, otherwise username or email)
        /// </summary>
        public string DisplayName =>
            !string.IsNullOrWhiteSpace(FullName) ? FullName :
            !string.IsNullOrWhiteSpace(Username) ? Username :
            Email;

        /// <summary>
        /// Checks if user is an admin
        /// </summary>
        public bool IsAdmin => GlobalRole == UserRole.Admin;

        /// <summary>
        /// Checks if user is an organizer
        /// </summary>
        public bool IsOrganizer => GlobalRole == UserRole.Organizer;

        /// <summary>
        /// Checks if user is a regular user
        /// </summary>
        public bool IsRegularUser => GlobalRole == UserRole.RegularUser;
    }
}
