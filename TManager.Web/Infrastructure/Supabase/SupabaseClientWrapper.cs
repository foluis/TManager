using Microsoft.Extensions.Options;
using Supabase;

namespace TManager.Web.Infrastructure.Supabase
{
    public class SupabaseClientWrapper
    {
        private readonly Client _client;
        private readonly SupabaseConfig _config;

        public SupabaseClientWrapper(IOptions<SupabaseConfig> config)
        {
            _config = config.Value;

            if (!_config.IsValid())
            {
                throw new InvalidOperationException(
                    "Supabase configuration is invalid. Please check your appsettings.json");
            }

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            _client = new Client(_config.Url, _config.Key, options);
        }

        /// <summary>
        /// Gets the underlying Supabase client
        /// </summary>
        public Client Client => _client;

        /// <summary>
        /// Initializes the Supabase client connection
        /// </summary>
        public async Task InitializeAsync()
        {
            await _client.InitializeAsync();
        }

        /// <summary>
        /// Tests the connection to Supabase
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await InitializeAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets connection status information
        /// </summary>
        public string GetConnectionInfo()
        {
            return $"Connected to: {_config.Url}";
        }
    }
}
