using System.Text.Json.Serialization;
using AdsWithQuartz.AutoMapper;
using AdsWithQuartz.BackgroundServices;
using AdsWithQuartz.Data;
using AdsWithQuartz.Middleware;
using AdsWithQuartz.QuartzJobs;
using AdsWithQuartz.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:7000");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(conf => conf.UseNpgsql(connection));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAdService, AdService>();

//Added BgService
builder.Services.AddHostedService<AdDeactivationBackgroundService>();

//Used Quartz
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("DeleteOldInactiveAdsJob");

    q.AddJob<DeleteOldInactiveAdsJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DeleteOldInactiveAdsJob-trigger")
        .StartNow()
        .WithSimpleSchedule(x =>
            x.WithIntervalInMinutes(1)
                .RepeatForever()
        )
    );
});

builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
});
// -----------------------------------------------------------------

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
await dbContext.Database.GetInfrastructure().GetService<IMigrator>()!.MigrateAsync();

app.Run();