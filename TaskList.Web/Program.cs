using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskList.Web
{
    public class Program
    {
        public static string EnvironmentName =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting TaskList.Web...");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
