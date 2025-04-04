using AutoMapper;
using equilog_backend.Models;
using equilog_backend.DTOs;

namespace equilog_backend.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Horse, HorseDto>();
        }
    }
}
