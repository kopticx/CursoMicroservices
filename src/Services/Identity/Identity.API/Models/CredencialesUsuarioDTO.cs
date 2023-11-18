using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models;

public class CredencialesUsuarioDTO
{
  [Required]
  public string UserName { get; set; }
  [Required]
  [EmailAddress]
  public string Email { get; set; }
}