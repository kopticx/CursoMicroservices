﻿using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService) : ControllerBase
{
  [HttpGet("{userName}", Name = "GetBasket")]
  [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> GetBasket(string userName)
  {
    var basket = await repository.GetBasket(userName);

    return Ok(basket ?? new ShoppingCart(userName));
  }

  [HttpPost]
  [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
  {
    foreach (var item in basket.Items)
    {
      var coupon = await discountGrpcService.GetDiscount(item.ProductName);
      item.Price -= coupon.Amount;
    }

    return Ok(await repository.UpdateBasket(basket));
  }

  [HttpDelete("{userName}", Name = "DeleteBasket")]
  [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> DeleteBasket(string userName)
  {
    await repository.DeleteBasket(userName);

    return Ok();
  }
}