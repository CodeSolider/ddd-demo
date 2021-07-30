using EbayPlatform.Application.Dto;
using EbayPlatform.Application.IntegrationEvents;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Dependency;
using EbayPlatform.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EbayPlatform.WebApi.Extensions
{
    /// <summary>
    /// 服务容器扩展类
    /// </summary>
    internal static class ServiceCollectionExtension
    {
        /// <summary>
        /// MediatR
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        internal static IServiceCollection AddMediatRServices(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(StudentContextTransactionBehavior<,>));
            return services.AddMediatR(typeof(StudentDto).Assembly, typeof(Program).Assembly);
        }

        /// <summary>
        /// Domain 层注入数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IServiceCollection AddDomainContext(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<StudentDbContext>(options);
            //事务注入
            services.AddTransient<ITransaction, Transaction<StudentDbContext>>();
            return services;
        }

        /// <summary>
        ///  添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        internal static IServiceCollection AddSqlServerDomainContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new NullReferenceException($"{nameof(connectionString)}不能为空");

            return services.AddDomainContext(builder =>
            {
                builder.UseSqlServer(connectionString);
            });
        }

        /// <summary>
        /// 添加分布式事件总线
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCap(options =>
            {
                options.UseEntityFramework<StudentDbContext>();
                //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.UseRabbitMQ(opts =>
                {
                    configuration.GetSection("RabbitMQ").Bind(opts);
                });
                //设置重试次数
                options.FailedRetryCount = 5;
                //失败后的重拾间隔，默认60秒
                options.FailedRetryInterval = 60;
                // options.UseDashboard();
            });
            return services;
        }


        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        internal static IServiceCollection AddAutoDIService(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
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
