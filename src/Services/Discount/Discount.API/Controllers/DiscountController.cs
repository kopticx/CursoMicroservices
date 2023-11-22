using System.Net;
using AutoMapper;
using Discount.API.Entities;
using Discount.API.Models;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/discount")]
public class DiscountController(IDiscountRepository repository, IMapper mapper) : ControllerBase
{
  [HttpGet("{productName}", Name = "GetDiscount")]
  [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.NotFound)]
  public async Task<IActionResult> GetDiscount(string productName)
  {
    var coupon = await repository.GetDiscount(productName);

    if (coupon is null)
    {
      return NotFound("El cupón no existe");
    }

    return Ok(coupon);
  }
  
  [HttpPost]
  [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.Created)]
  [ProducesResponseType((int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult> CreateDiscount(CouponDTO model)
  {
    var coupon = mapper.Map<Coupon>(model);

    var result = await repository.CreateDiscount(coupon);

    if (result is false)
    {
      return BadRequest("No se pudo crear el cupón");
    }

    return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
  }

  [HttpPut]
  [ProducesResponseType((int)HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult> UpdateDiscount(Coupon coupon)
  {
    var result = await repository.UpdateDiscount(coupon);

    if (result is false)
    {
      return BadRequest("No se pudo actualizar el cupón");
    }

    return Ok();
  }

  [HttpDelete("{productName}", Name = "DeleteDiscount")]
  [ProducesResponseType((int) HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult> DeleteDiscount(string productName)
  {
    var result = await repository.DeleteDiscount(productName);

    if (result is false)
    {
      return BadRequest("No se pudo eliminar el cupón");
    }

    return Ok();
  }
}