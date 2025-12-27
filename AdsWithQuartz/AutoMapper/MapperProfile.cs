using AdsWithQuartz.DTOs;
using AdsWithQuartz.Entities;
using AutoMapper;

namespace AdsWithQuartz.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateAdDto, Ad>();
        CreateMap<UpdateAdDto, Ad>();

        CreateMap<CreateCategoryDto, Category>().ReverseMap();
    }
}