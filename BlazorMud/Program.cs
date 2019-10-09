using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;

namespace BlazorMud
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });

            var additionalJsonConfigurationViaEnv = System.Environment.GetEnvironmentVariable("BLAZORMUD_CONFIG");
            if (additionalJsonConfigurationViaEnv != null)
            {
                var pathToAdditionalJson = additionalJsonConfigurationViaEnv.Split('=')[1];
                builder.ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile(pathToAdditionalJson, optional: true, reloadOnChange: true));
            }

            var additionalJsonConfigurationViaArgs = args.FirstOrDefault(x => x.StartsWith("BLAZORMUD_CONFIG=", System.StringComparison.OrdinalIgnoreCase));
            if (additionalJsonConfigurationViaArgs != null)
            {
                var pathToAdditionalJson = additionalJsonConfigurationViaArgs.Split('=')[1];
                builder.ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile(pathToAdditionalJson, optional: true, reloadOnChange: true));
            }

            return builder;
        }
    }
}
