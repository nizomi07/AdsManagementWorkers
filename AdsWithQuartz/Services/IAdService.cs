using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AdsWithQuartz.Responses;

namespace AdsWithQuartz.Services;

public interface IAdService
{
    Task<Response<Ad>> CreateAsync(CreateAdDto dto);
    Task<Response<Ad>> UpdateAsync(UpdateAdDto dto);
    Task<Response<List<Ad>>> GetAllAsync();
    Task DeleteAsync(int id);
}