using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Data;

public class EquilogDbContext(DbContextOptions<EquilogDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Horse> Horses { get; set; }
    public DbSet<Stable> Stables { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<UserStable> UserStables { get; set; }
    public DbSet<UserHorse> UserHorses { get; set; }
    public DbSet<StableHorse> StableHorses { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }
    public DbSet<StablePost> StablePosts { get; set; }
}