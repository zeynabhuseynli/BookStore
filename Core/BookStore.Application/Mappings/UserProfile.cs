using AutoMapper;
using BookStore.Application.DTOs.UserDtos;
using BookStore.Domain.Entities.Users;

namespace BookStore.Application.Mappings;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDay));
    }
}

