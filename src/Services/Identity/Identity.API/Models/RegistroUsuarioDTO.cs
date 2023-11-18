using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models;

public class RegistroUsuarioDTO
{
  [Required]
  public string UserName { get; set; }
  [Required]
  [EmailAddress]
  public string Email { get; set; }
  [Required]
  public string Password { get; set; }
}