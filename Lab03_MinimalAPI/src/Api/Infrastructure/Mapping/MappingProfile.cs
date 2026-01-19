using AutoMapper;
using Lab03_MinimalAPI.Domain;
using Lab03_MinimalAPI.DTOs;

namespace Lab03_MinimalAPI.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserRegisterDto, User>();

        CreateMap<Role, RoleDto>();
        CreateMap<UserRoleAssignDto, UserRole>();

    }
}
