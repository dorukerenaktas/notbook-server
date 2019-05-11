using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NotBook.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = null;
                })
                .UseStartup<Startup>();
        }
    }
}