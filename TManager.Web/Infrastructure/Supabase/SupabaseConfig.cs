namespace TManager.Web.Infrastructure.Supabase
{
    public class SupabaseConfig
    {
        public string Url { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Validates that all required configuration values are present
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Url) &&
                   !string.IsNullOrWhiteSpace(Key) &&
                   Url.StartsWith("https://");
        }
    }
}
