using AutoMapper;
using equilog_backend.Models;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Horse, HorseDto>().ReverseMap();
            CreateMap<HorseUpdateDto, Horse>();
            CreateMap<HorseCreateDto, HorseDto>();
            CreateMap<HorseCreateDto, Horse>();

            CreateMap<CalendarEvent, CalendarEventDto>().ReverseMap();
            CreateMap<CalendarEventCreateDto, CalendarEvent>();
            CreateMap<CalendarEventUpdateDto, CalendarEventDto>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserCreateDto, UserDto>();
            CreateMap<UserCreateDto, User>();
        }
    }
}
