using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class PasswordResetService(EquilogDbContext context, IMapper mapper) : IPasswordResetService
{
    public async Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email)
    {
        try
        {
            if (!await context.Users
                    .AnyAsync(u => u.Email == email))
                return ApiResponse<PasswordResetRequestDto?>.Failure(HttpStatusCode.BadRequest,
                    $"Account with the email {email} does not exist");
            
            var oldPasswordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Email == email)
                .FirstOrDefaultAsync();

            if (oldPasswordResetRequest != null)
            {
                context.PasswordResetRequests.Remove(oldPasswordResetRequest);
                await context.SaveChangesAsync();
            }

            var passwordResetRequest = new PasswordResetRequest()
            {
                Email = email,
                ResetCode = Generate.PasswordResetCode(),
                ExpirationDate = DateTime.Now.AddHours(24)
            };

            context.PasswordResetRequests.Add(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<PasswordResetRequestDto?>.Success(HttpStatusCode.Created,
                mapper.Map<PasswordResetRequestDto>(passwordResetRequest),
                "Password reset request was created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetRequestDto?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<ValidateResetCodeDto?>> ValidateResetCodeAsync(ValidateResetCodeDto validateResetCodeDto)
    {
        try
        {
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Email == validateResetCodeDto.Email)
                .FirstOrDefaultAsync();
        
            if (passwordResetRequest == null)
                return ApiResponse<ValidateResetCodeDto?>.Failure(HttpStatusCode.NotFound,
                    "A password reset request for this account does not exist. Try creating a new one.");

            if (passwordResetRequest.ResetCode != validateResetCodeDto.ResetCode)
                return ApiResponse<ValidateResetCodeDto?>.Failure(HttpStatusCode.Unauthorized,
                    "Invalid reset code.");
            
            context.PasswordResetRequests.Remove(passwordResetRequest);
            await context.SaveChangesAsync();
                
            return ApiResponse<ValidateResetCodeDto?>.Success(HttpStatusCode.OK,
                null,
                "Reset code validated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<ValidateResetCodeDto?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<PasswordResetDto?>> ResetPasswordAsync(PasswordResetDto passwordResetDto)
    {
        try
        {
            if (passwordResetDto.NewPassword != passwordResetDto.NewPasswordConfirmation)
                return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.BadRequest,
                    "Passwords have to match.");
        
            var user = await context.Users
                .Where(u => u.Email == passwordResetDto.Email)
                .FirstOrDefaultAsync();
        
            if (user == null)
                return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.NotFound,
                    $"Account tied to email {passwordResetDto.Email} does not exist.");
        
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordResetDto.NewPassword, salt);

            user.PasswordHash = passwordHash;
            await context.SaveChangesAsync();

            return ApiResponse<PasswordResetDto?>.Success(HttpStatusCode.OK,
                null,
                "Password was reset successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<PasswordResetRequestDto?>> DeletePasswordResetRequestAsync(int id)
    {
        try
        {
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Id == id)
                .FirstOrDefaultAsync();
            
            if (passwordResetRequest == null)
                return ApiResponse<PasswordResetRequestDto?>.Failure(HttpStatusCode.NotFound,
                    "Error: Password reset request not found");

            context.PasswordResetRequests.Remove(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<PasswordResetRequestDto?>.Success(HttpStatusCode.NoContent,
                null,
                "Password reset request deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetRequestDto?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}