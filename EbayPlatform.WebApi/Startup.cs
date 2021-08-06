using EbayPlatform.Application.Dto;
using EbayPlatform.Application.IntegrationEvents;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Extensions;
using EbayPlatform.Infrastructure.Quartz;
using EbayPlatform.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace EbayPlatform.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerDocumentation();

            #region MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EbayPlatformContextTransactionBehavior<,>));
            services.AddMediatR(typeof(StudentDto).Assembly, typeof(Program).Assembly);
            #endregion

            services.AddSqlServerDomainContext<EbayPlatformDbContext>(Configuration.GetConnectionString("DefaultConnection"));
            services.AddAutoDIService();

            #region Cap
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCapService<EbayPlatformDbContext>(Configuration);
            #endregion

            #region Quartz
            services.UseQuartz();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IScheduler scheduler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }

            #region Quartz
            ISyncTaskJobConfigRepository syncTaskJobConfigRepository = app.ApplicationServices.GetRequiredService<ISyncTaskJobConfigRepository>();
            var syncTaskJobConfigList = syncTaskJobConfigRepository.GetSyncTaskJobConfigList();
            syncTaskJobConfigList.ForEach(syncTaskJobConfigItem =>
            {
                SchedulerProvider.StartJob(scheduler, syncTaskJobConfigItem);
            });
            #endregion


            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
