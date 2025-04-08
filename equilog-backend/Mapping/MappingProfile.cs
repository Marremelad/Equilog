using AutoMapper;
using equilog_backend.Models;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            
            CreateMap<Horse, HorseDto>().ReverseMap();
            CreateMap<HorseCreateDto, Horse>();
            CreateMap<HorseUpdateDto, Horse>();
            
            CreateMap<Stable, StableDto>().ReverseMap();
            CreateMap<StableCreateDto, Stable>();
            CreateMap<StableUpdateDto, Stable>();

            CreateMap<StablePost, StablePostDto>().ReverseMap();
            CreateMap<StablePostCreateDto, StablePost>();
            CreateMap<StablePostUpdateDto, StablePost>();

            CreateMap<CalendarEvent, CalendarEventDto>().ReverseMap();
            CreateMap<CalendarEventCreateDto, CalendarEvent>();
            CreateMap<CalendarEventUpdateDto, CalendarEvent>();
        }
    }
}
