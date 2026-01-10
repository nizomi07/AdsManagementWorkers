using AdsWithQuartz.Data;
using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace UnitTest.CategoryTests;

public abstract class CategoryServiceTestBase
{
    protected readonly DataContext Context;
    protected readonly IMapper Mapper;
    protected readonly Mock<IMemoryCache> InMemoryCache;
    protected readonly string CategoriesCacheKey = "categories_all";

    protected CategoryServiceTestBase()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=ads_test_db;Username=postgres;Password=1zz4tull0_"
            )
            .Options;

        Context = new DataContext(options);
        
        // Context.Database.EnsureDeleted();
        Context.Database.Migrate();
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateCategoryDto, Category>();
        });

        Mapper = mapperConfig.CreateMapper();
        InMemoryCache = new Mock<IMemoryCache>();

    }

    protected CategoryService CreateService()
        => new CategoryService(Context, Mapper, InMemoryCache.Object);
}