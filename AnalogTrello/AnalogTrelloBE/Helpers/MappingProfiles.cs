using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Models;
using AutoMapper;
using TaskScheduler = AnalogTrelloBE.Models.TaskScheduler;

namespace AnalogTrelloBE.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<TaskSchedulerDto, TaskScheduler>()
            .ForMember(x => x.CreatedAt, 
                y => y.MapFrom(src => DateTime.Now))
            .ReverseMap();
    }
    
}