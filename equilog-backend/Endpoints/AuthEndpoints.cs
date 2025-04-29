using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class AuthEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Register.
        app.MapPost("/api/auth/register", Register)
            .AddEndpointFilter<ValidationFilter<RegisterDto>>()
            .WithName("Register");

        // Log in.
        app.MapPost("/api/auth/login", Login)
            .AddEndpointFilter<ValidationFilter<LoginDto>>()
            .WithName("Login");
        
        // Refresh token.
        app.MapPost("/api/auth/refresh-token", RefreshToken)
            .WithName("RefreshToken");
        
        // Log out.
        app.MapPost("/api/auth/revoke-token", LogOut)
            .WithName("RevokeToken");
    }

    private static async Task<IResult> Register(IAuthService authService, RegisterDto registerDto)
    {
        return Result.Generate(await authService.RegisterAsync(registerDto));
    }

    private static async Task<IResult> Login(IAuthService authService, LoginDto loginDto)
    {
        return Result.Generate(await authService.LoginAsync(loginDto));
    }
    
    private static async Task<IResult> RefreshToken(IAuthService authService, RefreshTokenRequestDto refreshTokenRequest)
    {
        return Result.Generate(await authService.RefreshTokenAsync(refreshTokenRequest.RefreshToken));
    }
    
    private static async Task<IResult> LogOut(IAuthService authService, RevokeTokenRequestDto revokeTokenRequest)
    {
        return Result.Generate(await authService.RevokeRefreshTokenAsync(revokeTokenRequest.RefreshToken));
    }
}