using AutoMapper;
using equilog_backend.Models;
using equilog_backend.DTOs;

namespace equilog_backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Horse, HorseDto>();
        }
    }
}
