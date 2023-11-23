using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Utilities;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<Coupon, CouponModel>().ReverseMap();
    CreateMap<CreateDiscountRequest, Coupon>().ReverseMap();
    CreateMap<UpdateDiscountRequest, Coupon>().ReverseMap();
    CreateMap<bool, DeleteDiscountResponse>().ReverseMap();
  }
}