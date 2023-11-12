using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models;

namespace Catalog.API.Services;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<Product, ProductDTO>().ReverseMap();
  }
}