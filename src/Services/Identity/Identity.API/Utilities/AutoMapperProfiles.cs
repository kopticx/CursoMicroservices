using AutoMapper;
using Identity.API.Models;

namespace Identity.API.Utilities;

public class AutoMapperProfiles : Profile
{
  public AutoMapperProfiles()
  {
    CreateMap<CredencialesUsuarioDTO, RegistroUsuarioDTO>().ReverseMap();
    CreateMap<CredencialesUsuarioDTO, LoginDTO>().ReverseMap();
  }
}