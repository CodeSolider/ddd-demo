using EbayPlatform.WebApi.HostedService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EbayPlatform.WebApi
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class Program
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static void Main(string[] args)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            CreateHostBuilder(args).Build().Run();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static IHostBuilder CreateHostBuilder(string[] args) =>
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddNLog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // add commands
                    new ConfigurationBuilder().AddCommandLine(args).Build();
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices(services =>
                {
                    //将Quartz托管
                    services.AddHostedService<QuartzHostedService>();
                });
    }
}
