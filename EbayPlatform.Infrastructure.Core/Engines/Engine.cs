using EbayPlatform.Domain.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EbayPlatform.Infrastructure.Core.Engines
{
    /// <summary>
    /// 引擎类
    /// </summary>
    public class Engine : IEngine
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        private readonly IServiceProvider serviceProvider;
        public Engine(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        public T Resolve<T>() where T : class
        {
            using var serviceScope = serviceProvider.CreateScope();
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
