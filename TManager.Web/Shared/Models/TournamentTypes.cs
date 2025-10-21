namespace TManager.Web.Shared.Models
{
    /// <summary>
    /// Tournament type/format constants
    /// </summary>
    public static class TournamentTypes
    {
        public const string SingleElimination = "single_elimination";
        public const string DoubleElimination = "double_elimination";
        public const string RoundRobin = "round_robin";
        public const string Swiss = "swiss";

        /// <summary>
        /// Gets all valid tournament types
        /// </summary>
        public static readonly string[] AllTypes = new[]
        {
        SingleElimination,
        DoubleElimination,
        RoundRobin,
        Swiss
    };

        /// <summary>
        /// Gets display name for tournament type
        /// </summary>
        public static string GetDisplayName(string type) => type switch
        {
            SingleElimination => "Single Elimination",
            DoubleElimination => "Double Elimination",
            RoundRobin => "Round Robin",
            Swiss => "Swiss System",
            _ => "Unknown"
        };

        /// <summary>
        /// Gets description for tournament type
        /// </summary>
        public static string GetDescription(string type) => type switch
        {
            SingleElimination => "One loss and you're out. Fast-paced bracket format.",
            DoubleElimination => "Two chances before elimination. Winner's and loser's brackets.",
            RoundRobin => "Everyone plays everyone. Most matches, fairest results.",
            Swiss => "Pairing based on performance. Balance of speed and fairness.",
            _ => ""
        };

        /// <summary>
        /// Validates if a tournament type string is valid
        /// </summary>
        public static bool IsValid(string type) => AllTypes.Contains(type);
    }
}
