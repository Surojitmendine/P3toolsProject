using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using API.Context;
using NLog.Web;
using System;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

            var host = new HostBuilder()                
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // JSON files, User secrets, environment variables and command line arguments                    
                    config.AddJsonFile(
                        "appsettings.json", optional: true, reloadOnChange: true);
                   
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(serverOptions =>
                    {
                        // Set properties and call methods on options
                        serverOptions.ListenLocalhost(5000);
                        
                    });
                    //webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                    
                })
               
                .Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    // Set properties and call methods on options
                })
                //.UseIISIntegration()
                .UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            })           
            .UseNLog();  // NLog: Setup NLog for Dependency injection

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        })              
        .UseNLog();  // NLog: setup NLog for Dependency injection

       

    }
}
