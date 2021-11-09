using EbayPlatform.WebApi.HostedService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace EbayPlatform.WebApi
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class Program
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
        /// <summary>
        /// Namespace
        /// </summary>
        readonly static string Namespace = typeof(Startup).Namespace;

        /// <summary>
        /// AppName
        /// </summary>
        readonly static string AppName = Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static int Main(string[] args)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            Log.Logger = CreateSerilogLogger(GetConfiguration());
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static IHostBuilder CreateHostBuilder(string[] args) =>
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // add commands
                    new ConfigurationBuilder().AddCommandLine(args).Build();
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    //将Quartz托管
                    services.AddHostedService<QuartzHostedService>();
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog();

        static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   .Enrich.WithProperty("ApplicationContext", AppName)
                   .Enrich.FromLogContext()
                   .WriteTo.Console()
                   .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                   .ReadFrom.Configuration(configuration)
                   .CreateLogger();
        }

        static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .Build();
        }
    }
}
