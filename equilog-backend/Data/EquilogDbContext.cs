using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Data;

public class EquilogDbContext(DbContextOptions<EquilogDbContext> options)
    :DbContext(options)
{
    
}