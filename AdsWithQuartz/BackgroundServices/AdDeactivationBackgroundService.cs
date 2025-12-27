using AdsWithQuartz.Data;
using Microsoft.EntityFrameworkCore;

namespace AdsWithQuartz.BackgroundServices;

public class AdDeactivationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AdDeactivationBackgroundService> _logger;

    public AdDeactivationBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<AdDeactivationBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ad deactivation worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var now = DateTime.UtcNow;

                var expiredAds = await db.Ads
                    .Where(ad => ad.IsActive && ad.ActiveUntil < now)
                    .ToListAsync(stoppingToken);

                if (expiredAds.Any())
                {
                    foreach (var ad in expiredAds)
                    {
                        ad.IsActive = false;
                        ad.DeactivatedAt = now;
                    }

                    await db.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation(
                        "Deactivated {Count} expired ads at {Time}",
                        expiredAds.Count,
                        now);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deactivating ads");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
