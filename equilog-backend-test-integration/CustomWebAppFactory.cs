using equilog_backend;
using equilog_backend.Data;
using equilog_backend_test_integration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    private readonly bool _useFakeAuth;

    public CustomWebAppFactory(bool useFakeAuth = false)
    {
        _useFakeAuth = useFakeAuth;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace DB with In-Memory
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<EquilogDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<EquilogDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Optional: Add fake auth if _useFakeAuth is true
            if (_useFakeAuth)
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "Test", options => { });
            }

            // Ensure test DB is fresh
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EquilogDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });

        builder.ConfigureTestServices(services =>
        {
            if (_useFakeAuth)
            {
                // Default to using "Test" auth scheme if it's enabled
                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });
            }
        });
    }
}

