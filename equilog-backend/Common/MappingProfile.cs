using AutoMapper;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.PasswordDTOs;
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

            CreateMap<Stable, StableDto>().ReverseMap();
            CreateMap<Stable, StableSearchDto>(MemberList.Source);
            CreateMap<StableCreateDto, Stable>(MemberList.Source);
            CreateMap<StableUpdateDto, Stable>(MemberList.Source);

            CreateMap<StablePost, StablePostDto>().ReverseMap();
            CreateMap<StablePostCreateDto, StablePost>(MemberList.Source);
            CreateMap<StablePostUpdateDto, StablePost>(MemberList.Source);

            CreateMap<CalendarEvent, CalendarEventDto>().ReverseMap();
            CreateMap<CalendarEventCreateDto, CalendarEvent>(MemberList.Source);
            CreateMap<CalendarEventUpdateDto, CalendarEvent>(MemberList.Source);

            CreateMap<WallPost, WallPostDto>().ReverseMap();
            CreateMap<WallPostReplaceDto, WallPost>(MemberList.Source);
            CreateMap<WallPostEditDto, WallPost>(MemberList.Source);
            CreateMap<WallPostClearDto, WallPost>(MemberList.Source);

            CreateMap<PasswordResetRequest, PasswordResetRequestDto>().ReverseMap();

            CreateMap<UserStable, UserStableDto>();
        }
    }
}
