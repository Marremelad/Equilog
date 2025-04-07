using AutoMapper;
using equilog_backend.Models;
using equilog_backend.DTOs;
using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            
            CreateMap<Horse, HorseDto>().ReverseMap();
            CreateMap<HorseUpdateDto, Horse>();
            CreateMap<HorseCreateDto, HorseDto>();
            CreateMap<HorseCreateDto, Horse>();

            CreateMap<Event, EventDto>().ReverseMap();
        }
    }
}
