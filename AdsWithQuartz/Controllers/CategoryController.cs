using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdsWithQuartz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Category>> CreateAsync(CreateCategoryDto dto)
    {
        var createCategory = await service.CreateAsync(dto);
        return Ok(createCategory);
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAllAsync()
    {
        var categories = await service.GetAllAsync();
        return Ok(categories);
    }
}