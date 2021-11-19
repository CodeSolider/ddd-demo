using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Extensions;
using EbayPlatform.WebApi.Extensions;
using EbayPlatform.WebApi.HostedService;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//host BackgroundService
builder.Host.ConfigureServices(services =>
{
    //½«QuartzÍÐ¹Ü
    services.AddHostedService<QuartzHostedService>();
});

// Add services to the container.
#region  Add services to the container. 
builder.Services.AddSqlServerDomainContext<EbayPlatformDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"), serviceLifetime: ServiceLifetime.Transient)
        .AddAutoDIService()
        .AddCapEventBus<EbayPlatformDbContext>(builder.Configuration)
        .Services
        .AddSwaggerDocumentation()
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(EbayPlatformContextTransactionBehavior<,>))
        .AddMediatR(Assembly.Load(builder.Configuration.GetSection("MediatorPath").Value), typeof(Program).Assembly)
        .AddEngineService()
        .UseQuartz();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
