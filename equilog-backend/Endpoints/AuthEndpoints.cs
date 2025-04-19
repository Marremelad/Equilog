using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using equilog_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace equilog_backend.Endpoints;

public class AuthEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Request verification code.
        app.MapPost("/api/auth/register/send-code", SendCode)
            .WithName("RequestCode")
            .AllowAnonymous();

        // Verify verification code.
        app.MapPost("/api/auth/register/verify-code", VerifyCode)
            .AddEndpointFilter<ValidationFilter<CompleteRegistrationDto>>()
            .WithName("VerifyCode")
            .AllowAnonymous();

        // Register endpoint.
        app.MapPost("/api/auth/register", Register)
            .AddEndpointFilter<ValidationFilter<RegisterDto>>()
            .WithName("Register");

        // Login endpoint.
        app.MapPost("/api/auth/login", Login)
            .AddEndpointFilter<ValidationFilter<LoginDto>>()
            .WithName("Login");
    }

    private static async Task<IResult> SendCode(
        TwilioService twilio,
        EquilogDbContext db,
        string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            return Results.BadRequest(new { message = "No email provided" });
        }

        bool userExists = await db.Users.AnyAsync(u => u.Email == userEmail);
        if (userExists)
            return Results.Conflict(new { message = "A user with this email already exists." });

        bool sent = await twilio.SendVerificationCodeAsync(userEmail);
        if (!sent)
            return TypedResults.Json(
                new { message = "Failed to send verification code." },
                statusCode: StatusCodes.Status500InternalServerError
            );

        return Results.Ok(new { message = "Verification code sent." });
    }

    private static async Task<IResult> VerifyCode(
        TwilioService twilio,
        EquilogDbContext db,
        CompleteRegistrationDto req)
    {
        bool verified = await twilio.VerifyCodeAsync(req.Email, req.TwilioCode);
        if (!verified)
            return Results.BadRequest(new { message = "Invalid or expired verification code." });

        var user = new User
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            UserName = req.UserName,
            PhoneNumber = req.PhoneNumber
        };
        db.Users.Add(user);

        //var accessToken = tokenService.GenerateAccessToken(user);
        //var refreshToken = tokenService.GenerateRefreshToken();
        //db.RefreshTokens.Add(new RefreshToken
        //{
        //    ConnectedUser = user.Id,
        //    Token = refreshToken,
        //    ExpiresAt = DateTime.UtcNow.AddDays(30)
        //});

        await db.SaveChangesAsync();

        return Results.Ok(new { message = $"User {user.Id} has been created" });
    }

    private static async Task<IResult> Register(IAuthService authService, RegisterDto registerDto)
    {
        return Result.Generate(await authService.Register(registerDto));
    }

    private static async Task<IResult> Login(IAuthService authService, LoginDto loginDto)
    {
        return Result.Generate(await authService.Login(loginDto));
    }
}