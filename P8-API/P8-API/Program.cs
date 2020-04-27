using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using P8_API.Services;

namespace P8_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TripService ts = new TripService();
            ts.Test();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
