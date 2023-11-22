using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Identity.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
  UserManager<IdentityUser> userManager,
  SignInManager<IdentityUser> signInManager,
  IConfiguration configuration,
  IMapper mapper
) : ControllerBase
{
  [HttpPost("Registrar")]
  [ProducesResponseType(typeof(RespuestaAuthDTO), (int)HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult> Registrar(RegistroUsuarioDTO model)
  {
    var usuario = new IdentityUser { UserName = model.UserName, Email = model.Email };

    var resultado = await userManager.CreateAsync(usuario, model.Password);

    var usuarioModel = mapper.Map<CredencialesUsuarioDTO>(model);

    if (resultado.Succeeded)
    {
      return Ok(await ConstruirToken(usuarioModel));
    }

    return BadRequest(resultado.Errors);
  }

  [HttpPost("Login", Name = "loginUsuario")]
  [ProducesResponseType(typeof(RespuestaAuthDTO), (int)HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.BadRequest)]
  public async Task<IActionResult> Login(LoginDTO model)
  {
    var resultado =
      await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false,
        lockoutOnFailure: false);

    if (!resultado.Succeeded) return BadRequest("Login incorrecto");

    var usuario = await userManager.FindByNameAsync(model.UserName);

    var usuarioModel = mapper.Map<CredencialesUsuarioDTO>(model);
    usuarioModel.Email = usuario!.Email;

    return Ok(await ConstruirToken(usuarioModel));
  }

  [HttpGet("RenovarToken", Name = "renovarTokenUsuario")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [ProducesResponseType(typeof(RespuestaAuthDTO), (int)HttpStatusCode.OK)]
  [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
  public async Task<IActionResult> RenovarToken()
  {
    var emailClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
    var email = emailClaim.Value;
    var name = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

    var credencialesUsuario = new CredencialesUsuarioDTO()
    {
      Email = email,
      UserName = name
    };

    return Ok(await ConstruirToken(credencialesUsuario));
  }

  [HttpPost("HacerAdmin", Name = "hacerAdmin")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
  [ProducesResponseType((int)HttpStatusCode.NoContent)]
  public async Task<IActionResult> HacerAdmin(EditarAdminDTO model)
  {
    var usuario = await userManager.FindByEmailAsync(model.Email);
    await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, "admin"));

    return NoContent();
  }

  [HttpPost("RemoveAdmin", Name = "removeAdmin")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
  [ProducesResponseType((int)HttpStatusCode.NoContent)]
  public async Task<IActionResult> RemoveAdmin(EditarAdminDTO model)
  {
    var usuario = await userManager.FindByEmailAsync(model.Email);
    await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, "admin"));

    return NoContent();
  }

  private async Task<RespuestaAuthDTO> ConstruirToken(CredencialesUsuarioDTO model)
  {
    var claims = new List<Claim>()
    {
      new(ClaimTypes.Email, model.Email),
      new(ClaimTypes.Name, model.UserName)
    };

    var usuario = await userManager.FindByEmailAsync(model.Email);
    var claimsDb = await userManager.GetClaimsAsync(usuario);

    claims.AddRange(claimsDb);

    var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("LlaveJWT")));
    var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

    var expiracion = DateTime.UtcNow.AddYears(1);

    var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion,
      signingCredentials: credenciales);

    return new RespuestaAuthDTO()
    {
      Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
      Expiracion = expiracion
    };
  }
}