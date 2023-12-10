using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService(IDiscountRepository repository, IMapper mapper) : DiscountProtoService.DiscountProtoServiceBase
{
  public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
  {
    var coupon = await repository.GetDiscount(request.ProductName);

    if (coupon is null)
    {
      throw new RpcException(new Status(StatusCode.NotFound,
        $"Discount with ProductName={request.ProductName} is not found"));
    }

    var couponModel = mapper.Map<CouponModel>(coupon);

    return couponModel;
  }

  public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
  {
    var coupon = mapper.Map<Coupon>(request);

    await repository.CreateDiscount(coupon);

    var couponModel = mapper.Map<CouponModel>(coupon);

    return couponModel;
  }

  public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
  {
    var coupon = mapper.Map<Coupon>(request);

    await repository.UpdateDiscount(coupon);

    var couponModel = mapper.Map<CouponModel>(coupon);

    return couponModel;
  }

  public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
  {
    var response = await repository.DeleteDiscount(request.ProductName);

    var deleteDiscountResponse = mapper.Map<DeleteDiscountResponse>(response);

    return deleteDiscountResponse;
  }
}