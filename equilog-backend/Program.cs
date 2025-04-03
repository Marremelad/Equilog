using equilog_backend.Data;
using equilog_backend.Endpoints;
using equilog_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<EquilogDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddScoped<EventService>();
        builder.Services.AddScoped<HorseService>();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        EventEndpoints.RegisterEndpoints(app);
        HorseEndpoints.RegisterEndpoints(app);

        app.Run();
    }
}