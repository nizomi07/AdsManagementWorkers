using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdsWithQuartz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdController(IAdService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Ad>> CreateAsync(CreateAdDto dto)
    {
        var createAd = await service.CreateAsync(dto);
        return Ok(createAd);
    }

    [HttpPut]
    public async Task<ActionResult<Ad>> UpdateAsync(UpdateAdDto dto)
    {
        var updateAd = await service.UpdateAsync(dto);
        return Ok(updateAd);
    }

    [HttpDelete]
    public async Task<ActionResult<Ad>> DeleteAsync(int id)
    {
        await service.DeleteAsync(id);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<Ad>>> ListAsync()
    {
        var ads = await service.GetAllAsync();
        return Ok(ads);
    }
}