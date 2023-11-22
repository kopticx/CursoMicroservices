using System.ComponentModel.DataAnnotations;

namespace Discount.API.Models;

public class CouponDTO
{
  [Required]
  public string ProductName { get; set; }
  [Required]
  public string Description { get; set; }
  [Required]
  public int Amount { get; set; }
}