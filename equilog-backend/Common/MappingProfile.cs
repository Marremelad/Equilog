using AutoMapper;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Models;

namespace equilog_backend.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCreateDto, User>(MemberList.Source);
            CreateMap<UserUpdateDto, User>(MemberList.Source);
            CreateMap<RegisterDto, User>(MemberList.Source)
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate());

            CreateMap<Horse, HorseDto>().ReverseMap();
            CreateMap<HorseCreateDto, Horse>(MemberList.Source);
            CreateMap<HorseUpdateDto, Horse>(MemberList.Source);

            CreateMap<Stable, StableDto>()
                .ForMember(dest => dest.MemberCount, opt 
                    => opt.MapFrom(src => src.UserStables != null ? src.UserStables.Count : 0))
                .ForMember(dest => dest.HorseCount, opt 
                    => opt.MapFrom(src => src.StableHorses != null ? src.StableHorses.Count : 0))
                .ReverseMap();
            CreateMap<Stable, StableSearchDto>(MemberList.Source);
            CreateMap<StableCreateDto, Stable>(MemberList.Source);
            CreateMap<StableUpdateDto, Stable>(MemberList.Source);

            CreateMap<StablePost, StablePostDto>()
                .ForMember(dest => dest.PosterFirstName, opt 
                    => opt.MapFrom(src => src.User != null ? src.User.FirstName : null))
                .ForMember(dest => dest.PosterLastName, opt 
                    => opt.MapFrom(src => src.User != null ? src.User.LastName : null))
                .ForMember(dest => dest.UserId, opt =>
                    opt.MapFrom(src => src.User != null ? src.User.Id : 0))
                .ReverseMap();
            CreateMap<StablePostCreateDto, StablePost>(MemberList.Source);
            CreateMap<StablePostUpdateDto, StablePost>(MemberList.Source);

            CreateMap<StableCompositionCreateDto, StableCreateDto>();

            CreateMap<CalendarEvent, CalendarEventDto>().ReverseMap();
            CreateMap<CalendarEventCreateDto, CalendarEvent>(MemberList.Source);
            CreateMap<CalendarEventUpdateDto, CalendarEvent>(MemberList.Source);

            CreateMap<WallPost, WallPostDto>().ReverseMap();
            CreateMap<WallPostReplaceDto, WallPost>(MemberList.Source);
            CreateMap<WallPostEditDto, WallPost>(MemberList.Source);
            CreateMap<WallPostClearDto, WallPost>(MemberList.Source);

            CreateMap<PasswordResetRequest, PasswordResetRequestDto>().ReverseMap();

            CreateMap<UserStable, UserStableDto>();
            CreateMap<UserStable, StableUserDto>()
                .ForMember(dest => dest.UserStableId, opt 
                    => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt 
                    => opt.MapFrom(src => src.UserIdFk))
                .ForMember(dest => dest.FirstName, opt 
                    => opt.MapFrom(src => src.User != null ? src.User.FirstName : null))
                .ForMember(dest => dest.LastName, opt 
                    => opt.MapFrom(src => src.User != null ? src.User.LastName : null));
        }
    }
}
