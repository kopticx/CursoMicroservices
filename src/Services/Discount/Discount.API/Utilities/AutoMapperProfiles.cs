using AutoMapper;
using Discount.API.Entities;
using Discount.API.Models;

namespace Discount.API.Utilities;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<Coupon, CouponDTO>().ReverseMap();
  }
}