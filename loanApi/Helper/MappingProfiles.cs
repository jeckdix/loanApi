using AutoMapper;
using loanApi.Dtos;
using loanApi.Models;

namespace loanApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // POST DTO
            CreateMap<UserDto, RegisterUsers>();
            CreateMap<UserProfilePostDto, UserProfile>();
        }

    }
}
