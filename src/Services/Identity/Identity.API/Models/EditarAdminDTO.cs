using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models;

public class EditarAdminDTO
{
  [Required]
  [EmailAddress]
  public string Email { get; set; }
}