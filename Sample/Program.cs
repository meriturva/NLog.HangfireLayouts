using Hangfire;
using Hangfire.PerformContextAccessor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Sample.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfirePerformContextAccessor();

// Add Hangfire services.
//GlobalConfiguration.Configuration.UseInMemoryStorage();
GlobalConfiguration.Configuration.UseSqlServerStorage(@"Server=localhost,1433;Database=services;User Id=sa;Password=Loccioni123!;");
builder.Services.AddHangfire((serviceProvider, config) =>
{
    // Add filter to handle PerformContextAccessor
    config.UsePerformContextAccessorFilter(serviceProvider);
});

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

app.UseHangfireDashboard();

app.MapControllers();

//BackgroundJob.Enqueue<SimpleJob>(sj => sj.DoJobAsync());
RecurringJob.AddOrUpdate<SimpleJob>("test", sj => sj.DoJobAsync(), "*/5 * * * * *");

app.Run();