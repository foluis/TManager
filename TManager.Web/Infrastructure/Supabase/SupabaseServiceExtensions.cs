namespace TManager.Web.Infrastructure.Supabase
{
    public static class SupabaseServiceExtensions
    {
        /// <summary>
        /// Adds Supabase services to the dependency injection container
        /// </summary>
        public static IServiceCollection AddSupabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind Supabase configuration
            services.Configure<SupabaseConfig>(configuration.GetSection("Supabase"));

            // Register Supabase client as singleton
            services.AddSingleton<SupabaseClientWrapper>();

            return services;
        }
    }
}
