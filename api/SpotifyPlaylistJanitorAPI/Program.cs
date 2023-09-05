using SpotifyPlaylistJanitorAPIs;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI
{
    /// <summary>
    /// Entrypoint class for SpotifyPlaylistJanitorAPI
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// Entrypoint method for SpotifyPlaylistJanitorAPI
        /// </summary>
        /// <param name="args">Command line args</param>
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        /// <summary>
        /// This method gets called by the runtime. Use this method to setup hosts.
        /// </summary>
        /// <param name="args">Command line args</param>
        /// <returns>The HostBuilder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    (webBuilder) => webBuilder.UseStartup<Startup>());
    }
}