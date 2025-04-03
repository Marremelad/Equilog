using equilog_backend.Data;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;
using equilog_backend.Common;
using AutoMapper;
using equilog_backend.DTOs;
namespace equilog_backend.Services
{
    public class HorseService(EquilogDbContext context, IMapper mapper)
    {
        public async Task<Response<List<HorseDto>?>> GetAllHorses()
        {
            try
            {
                var horses = await context.Horses.ToListAsync();
                var horseDtos = mapper.Map<List<HorseDto>>(horses);

                return Response<List<HorseDto>>.Success(horseDtos);
            }
            catch (Exception ex)
            {
                return Response<List<HorseDto>>.Failure(ex.Message);
            }
        }
    }
}
    
