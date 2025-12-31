using AdsWithQuartz.Data;
using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Responses;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AdsWithQuartz.Services;

public class AdService(DataContext context, IMapper mapper, IMemoryCache memoryCache) : IAdService
{
    private const string AdsCacheKey = "ads_all";

    public async Task<Response<Ad>> CreateAsync(CreateAdDto dto)
    {
        var ad = mapper.Map<Ad>(dto);
        context.Ads.Add(ad);
        await context.SaveChangesAsync();
        return new Response<Ad>(ad);
    }

    public async Task<Response<Ad>> UpdateAsync(UpdateAdDto dto)
    {
        var ad = mapper.Map<Ad>(dto);
        context.Ads.Update(ad);
        await context.SaveChangesAsync();
        return new Response<Ad>(ad);
    }

    public async Task<Response<List<Ad>>> GetAllAsync()
    {
        if (!memoryCache.TryGetValue(AdsCacheKey, out List<Ad>? ads))
        {
            ads = await context.Ads.ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            memoryCache.Set(AdsCacheKey, ads, cacheOptions);
        }

        return new Response<List<Ad>>(ads!);
    }


    public async Task DeleteAsync(int id)
    {
        var ad = await context.Ads.FindAsync(id);
        context.Ads.Remove(ad);
        await context.SaveChangesAsync();
    }
}