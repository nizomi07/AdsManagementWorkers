using AdsWithQuartz.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace AdsWithQuartz.QuartzJobs;

public class DeleteOldInactiveAdsJob : IJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DeleteOldInactiveAdsJob> _logger;

    public DeleteOldInactiveAdsJob(
        IServiceScopeFactory scopeFactory,
        ILogger<DeleteOldInactiveAdsJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();

        var thresholdDate = DateTime.UtcNow.AddDays(-7);

        var adsToDelete = await db.Ads
            .Where(ad =>
                !ad.IsActive &&
                ad.DeactivatedAt != null &&
                ad.DeactivatedAt < thresholdDate)
            .ToListAsync();

        if (adsToDelete.Any())
        {
            db.Ads.RemoveRange(adsToDelete);
            await db.SaveChangesAsync();

            _logger.LogInformation(
                "Quartz deleted {Count} inactive ads older than 7 days",
                adsToDelete.Count);
        }
    }
}