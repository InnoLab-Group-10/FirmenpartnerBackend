using FirmenpartnerBackend.Configuration;

namespace FirmenpartnerBackend
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        // Handle requests up to 50 MB
                        // Can't put this into appsettings.json because it is only parsed later
                        options.Limits.MaxRequestBodySize = Constants.MaxRequestSize;
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}