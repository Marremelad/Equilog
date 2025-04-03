using AutoMapper;
using equilog_backend.Models;
using equilog_backend.DTOs;

namespace equilog_backend.Mapping
{
    public class HorseProfile : Profile
    {
        public HorseProfile()
        {
            CreateMap<Horse, HorseDto>();

            // If you ever need custom mapping (e.g. concatenating strings or formatting dates),
            // you can add ForMember configuration here.
        }
    }
}
