using equilog_backend.Data;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;
using equilog_backend.Common;
namespace equilog_backend.Services
{
    public class HorseService(EquilogDbContext context)
    {
        public async Task<Response<List<Horse>?>> GetAllHorses()
        {
            try
            {
                var horses = await context.Horses
                    .ToListAsync();

                return Response<List<Horse>>.Success(horses);
            }
            catch (Exception ex)
            {
                return Response<List<Horse>>.Failure(ex.Message);

            }
        }
    }
}
    
