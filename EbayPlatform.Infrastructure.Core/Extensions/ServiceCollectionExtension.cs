using EbayPlatform.Infrastructure.Core.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace EbayPlatform.Infrastructure.Core.Extensions
{
    /// <summary>
    /// 服务容器扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Domain 层注入数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        static IServiceCollection AddDomainContext<TDbContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TDbContext : EFContext
        {
            services.AddDbContext<TDbContext>(options, serviceLifetime);
            return services;
        }

        /// <summary>
        ///  添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlServerDomainContext<TDbContext>(this IServiceCollection services,
            string connectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
             where TDbContext : EFContext
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new NullReferenceException($"{nameof(connectionString)}不能为空");

            return services.AddDomainContext<TDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString, options =>
                                                        options.MigrationsAssembly
                                                        (typeof(TDbContext).GetTypeInfo().Assembly.GetName().Name));
            }, serviceLifetime);
        }

        /// <summary>
        /// 添加CAP服务
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCapEventBus<TDbContext>(this IServiceCollection services,
            IConfiguration Configuration)
              where TDbContext : EFContext
        {
            services.AddCap(options =>
            {
                options.UseEntityFramework<TDbContext>();

                options.UseRabbitMQ(opts =>
                 {
                     Configuration.GetSection("RabbitMQ").Bind(opts);
                 });
                //设置重试次数
                options.FailedRetryCount = 3;
                //失败后的重拾间隔，默认5秒
                options.FailedRetryInterval = 5;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //设置成功信息的删除时间默认24*3600
                options.SucceedMessageExpiredAfter = Configuration.GetValue<int>("SucceedMessageExpiredAfter");
                //options.UseDashboard();
            }).AddSubscribeFilter<CapSubscribeFilter>();
            return services;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoDIService(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetCurrentPathAssembly();
            foreach (var assembly in allAssemblies)
            {
                var types = assembly.GetTypes()
                    .Where(type => type.IsClass
                                   && type.BaseType != null
                                   && type.HasImplementedRawGeneric(typeof(IDependency)));
                foreach (var type in types)
                {
                    var interfaces = type.GetInterfaces();

                    var interfaceType = interfaces.FirstOrDefault(x => x.Name == $"I{type.Name}");
                    if (interfaceType == null)
                    {
                        interfaceType = type;
                    }
                    ServiceDescriptor serviceDescriptor = new(interfaceType, type, serviceLifetime);
                    if (!services.Contains(serviceDescriptor))
                    {
                        services.Add(serviceDescriptor);
                    }
                }
            }
            return services;
        }
    }
}
