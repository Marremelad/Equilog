using AutoMapper;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.DTOs.CommentDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.StableHorseDTOs;
using equilog_backend.DTOs.StableLocationDtos;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.UserHorseDTOs;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Models;

namespace equilog_backend.Common;

// AutoMapper profile defining all object mappings between DTOs and entity models.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User entity mappings - bidirectional and create/update DTOs.
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserCreateDto, User>(MemberList.Source);
        CreateMap<UserUpdateDto, User>(MemberList.Source);
        CreateMap<RegisterDto, User>(MemberList.Source)
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate());

        // Horse entity mappings - bidirectional and create/update DTOs.
        CreateMap<Horse, HorseDto>().ReverseMap();
        CreateMap<HorseCreateDto, Horse>(MemberList.Source);
        CreateMap<HorseUpdateDto, Horse>(MemberList.Source);

        // Stable entity mappings with calculated properties for counts.
        CreateMap<Stable, StableDto>()
            .ForMember(dest => dest.MemberCount, 
                opt => opt.MapFrom(src => src.UserStables != null ? src.UserStables.Count : 0))
            .ForMember(dest => dest.HorseCount, 
                opt => opt.MapFrom(src => src.StableHorses != null ? src.StableHorses.Count : 0))
            .ReverseMap();
        CreateMap<Stable, StableSearchDto>(MemberList.Destination);
        CreateMap<StableCreateDto, Stable>(MemberList.Source);
        CreateMap<StableUpdateDto, Stable>(MemberList.Source);

        // StablePost mappings with user information from navigation properties.
        CreateMap<StablePost, StablePostDto>()
            .ForMember(dest => dest.PosterFirstName, 
                opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : null))
            .ForMember(dest => dest.PosterLastName, 
                opt => opt.MapFrom(src => src.User != null ? src.User.LastName : null))
            .ForMember(dest => dest.ProfilePicture,
                opt =>  opt.MapFrom(src =>  src.User != null ? src.User.ProfilePicture : null))
            .ForMember(dest => dest.UserId, 
                opt => opt.MapFrom(src => src.User != null ? src.User.Id : 0))
            .ReverseMap();
        CreateMap<StablePostCreateDto, StablePost>(MemberList.Source);
        CreateMap<StablePostUpdateDto, StablePost>(MemberList.Source);

        // CalendarEvent mappings with user details from navigation properties.
        CreateMap<CalendarEvent, CalendarEventDto>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.User != null ? src.User.Id : 0))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : null))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.User != null ? src.User.LastName : null))
            .ForMember(dest => dest.ProfilePicture,
                opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePicture : null));
        CreateMap<CalendarEventCreateDto, CalendarEvent>(MemberList.Source);
        CreateMap<CalendarEventUpdateDto, CalendarEvent>(MemberList.Source);
        
        // Password reset request mappings.
        CreateMap<PasswordResetRequest, PasswordResetRequestDto>().ReverseMap();

        // User-Stable relationship mappings.
        CreateMap<UserStable, UserStableDto>();
        CreateMap<UserStable, StableUserDto>()
            .ForMember(dest => dest.UserStableId, 
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, 
                opt => opt.MapFrom(src => src.UserIdFk))
            .ForMember(dest => dest.FirstName, 
                opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : null))
            .ForMember(dest => dest.LastName, 
                opt => opt.MapFrom(src => src.User != null ? src.User.LastName : null))
            .ForMember(dest => dest.ProfilePicture, 
                opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePicture : null));

        // Stable-Horse relationship mappings.
        CreateMap<StableHorse, StableHorseDto>()
            .ForMember(dest => dest.StableHorseId, 
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.HorseId, 
                opt => opt.MapFrom(src => src.HorseIdFk));

        // Stable-Horse mappings with owner information.
        CreateMap<StableHorse, StableHorseOwnersDto>()
            .ForMember(dest => dest.HorseId, 
                opt => opt.MapFrom(src => src.Horse!.Id))
            .ForMember(dest => dest.HorseName, 
                opt => opt.MapFrom(src => src.Horse!.Name))
            .ForMember(dest => dest.HorseColor, 
                opt => opt.MapFrom(src => src.Horse!.Color))
            .ForMember(dest => dest.HorseOwners, 
                opt => opt.MapFrom(src => src.Horse!.UserHorses != null 
                    ? src.Horse.UserHorses
                        .Where(uh => uh.User != null && uh.UserRole == 0)
                        .Select(uh => uh.User!.FirstName + " " + uh.User.LastName)
                        .ToList() 
                    : new List<string>()));

        // Comment mappings with user information from the first associated user.
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserComments != null && src.UserComments.Count != 0
                ? src.UserComments.First().User!.Id
                : 0))
            .ForMember(dest => dest.ProfilePicture, 
                opt => opt.MapFrom(src => src.UserComments != null && src.UserComments.Count != 0
                    ? src.UserComments.First().User!.ProfilePicture 
                    : null))
            .ForMember(dest => dest.FirstName, 
                opt => opt.MapFrom(src => src.UserComments != null && src.UserComments.Count != 0
                    ? src.UserComments.First().User!.FirstName 
                    : null))
            .ForMember(dest => dest.LastName, 
                opt => opt.MapFrom(src => src.UserComments != null && src.UserComments.Count != 0
                    ? src.UserComments.First().User!.LastName 
                    : null));
        
        // User-Stable role mappings.
        CreateMap<UserStable, UserStableRoleDto>();

        // User-Horse relationship mappings.
        CreateMap<UserHorse, HorseWithUserHorseRoleDto>().ReverseMap();
        CreateMap<UserHorse, UserWithUserHorseRoleDto>();
        
        // Stable location mappings.
        CreateMap<StableLocation, StableLocationDto>().ReverseMap();
    }
}