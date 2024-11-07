using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Models;
using AutoMapper;

namespace AnalogTrelloBE.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
    
}