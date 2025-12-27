using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Responses;

namespace AdsWithQuartz.Services;

public interface ICategoryService
{
    Task<Response<Category>> CreateAsync(CreateCategoryDto dto);
    Task<Response<List<Category>>> GetAllAsync();
}