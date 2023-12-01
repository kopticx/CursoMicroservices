﻿using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
{
  public async Task<CouponModel> GetDiscount(string productName)
  {
    var discountRequest = new GetDiscountRequest { ProductName = productName };
    return await discountProtoServiceClient.GetDiscountAsync(discountRequest);
  }
}