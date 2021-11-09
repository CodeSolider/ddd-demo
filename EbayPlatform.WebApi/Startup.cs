using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Extensions;
using EbayPlatform.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace EbayPlatform.WebApi
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Startup Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().Services
                    .AddSqlServerDomainContext<EbayPlatformDbContext>(Configuration.GetConnectionString("DefaultConnection"), serviceLifetime: ServiceLifetime.Transient)
                    .AddAutoDIService()
                    .AddCapEventBus<EbayPlatformDbContext>(Configuration)
                    .Services
                    .AddSwaggerDocumentation()
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(EbayPlatformContextTransactionBehavior<,>))
                    .AddMediatR(Assembly.Load(Configuration.GetSection("MediatorPath").Value), typeof(Program).Assembly)
                    .AddEngineService()
                    .UseQuartz();

            services.AddHttpClient("ScitooErp", options =>
            {
                options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Configuration.GetValue<string>("ScitooErp:AuthToken"));
                options.BaseAddress = new Uri(Configuration.GetValue<string>("ScitooErp:ApiUrl"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
