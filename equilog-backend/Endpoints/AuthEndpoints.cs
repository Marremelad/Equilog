using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class AuthEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Register endpoint.
        app.MapPost("/api/auth/register", Register)
            .AddEndpointFilter<ValidationFilter<RegisterDto>>()
            .WithName("Register");

        // Login endpoint.
        app.MapPost("/api/auth/login", Login)
            .AddEndpointFilter<ValidationFilter<LoginDto>>()
            .WithName("Login");
    }

    private static async Task<IResult> Register(IAuthService authService, RegisterDto registerDto)
    {
        return Result.Generate(await authService.RegisterAsync(registerDto));
    }

    private static async Task<IResult> Login(IAuthService authService, LoginDto loginDto)
    {
        return Result.Generate(await authService.LoginAsync(loginDto));
    }
}