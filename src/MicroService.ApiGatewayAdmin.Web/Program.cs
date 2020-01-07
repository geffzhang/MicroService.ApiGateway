using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.InProcess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace MicroService.ApiGateway
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //CurrentDirectoryHelpers.SetCurrentDirectory();
            try
            {
                Log.Information("Starting web host.");
                BuildWebHostInternal(args)
                    .Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder BuildWebHostInternal(string[] args) =>
          Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webbuilder =>
            {
                webbuilder.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseSerilog();
            })
            .UseAutofac();
    }
}
