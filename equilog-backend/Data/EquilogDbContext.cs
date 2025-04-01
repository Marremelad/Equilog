using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Data;

public class EquilogDbContext(DbContextOptions<EquilogDbContext> options)
    :DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Horse> Horses { get; set; }
}