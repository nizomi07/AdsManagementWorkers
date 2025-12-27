using System.Net;
using AdsWithQuartz.Data;
using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Responses;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdsWithQuartz.Services;

public class CategoryService(DataContext context, IMapper mapper) : ICategoryService
{
    public async Task<Response<Category>> CreateAsync(CreateCategoryDto dto)
    {
        var category = mapper.Map<Category>(dto);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return new Response<Category>(HttpStatusCode.OK, "Added", category);
    }

    public async Task<Response<List<Category>>> GetAllAsync()
    {
        var categories = await context.Categories.ToListAsync();
        return new Response<List<Category>>(HttpStatusCode.OK,"Categories Listed Successfully!", categories);
    }
}