using AutoMapper;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Models;

namespace equilog_backend.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<RegisterDto, User>();
            
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

            CreateMap<WallPost, WallPostDto>().ReverseMap();
            CreateMap<WallPostReplaceDto, WallPost>();
            CreateMap<WallPostEditDto, WallPost>();
            CreateMap<WallPostClearDto, WallPost>();
        }
    }
}
