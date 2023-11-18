using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models;

public class LoginDTO
{
  [Required]
  public string UserName { get; set; }
  [Required]
  public string Password { get; set; }
}