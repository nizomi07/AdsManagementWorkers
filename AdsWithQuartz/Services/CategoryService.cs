using System.Net;
using AdsWithQuartz.Data;
using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Responses;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AdsWithQuartz.Services;

public class CategoryService(DataContext context, IMapper mapper, IMemoryCache memoryCache) : ICategoryService
{
    private const string CategoriesCacheKey = "categories_all";

    public async Task<Response<Category>> CreateAsync(CreateCategoryDto dto)
    {
        var category = mapper.Map<Category>(dto);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return new Response<Category>(HttpStatusCode.OK, "Added", category);
    }

    public async Task<Response<List<Category>>> GetAllAsync()
    {
        if (!memoryCache.TryGetValue(CategoriesCacheKey, out List<Category>? categories))
        {
            categories = await context.Categories.ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            memoryCache.Set(CategoriesCacheKey, categories, cacheOptions);
        }

        return new Response<List<Category>>(
            HttpStatusCode.OK,
            "Categories Listed Successfully!",
            categories!
        );
    }
}