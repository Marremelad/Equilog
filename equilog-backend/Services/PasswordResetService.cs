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
                return ApiResponse<PasswordResetRequestDto>.Failure(HttpStatusCode.BadRequest,
                    $"Account with the email {email} does not exist");
            
            var storedPasswordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Email == email)
                .FirstOrDefaultAsync();

            if (storedPasswordResetRequest != null)
            {
                context.PasswordResetRequests.Remove(storedPasswordResetRequest);
                await context.SaveChangesAsync();
            }

            var passwordResetRequest = new PasswordResetRequest()
            {
                Email = email,
                Token = Generate.PasswordResetCode(),
                ExpirationDate = DateTime.Now.AddHours(24)
            };

            context.PasswordResetRequests.Add(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<PasswordResetRequestDto>.Success(HttpStatusCode.Created,
                mapper.Map<PasswordResetRequestDto>(passwordResetRequest),
                "Password reset request was created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetRequestDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> ResetPasswordWithTokenAsync(PasswordResetWithTokenDto passwordResetWithTokenDto)
    {
        try
        {
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Token == passwordResetWithTokenDto.Token)
                .FirstOrDefaultAsync();
            
            if (passwordResetRequest == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Invalid reset token.");
            
            if (passwordResetRequest.ExpirationDate < DateTime.Now)
            {
                context.PasswordResetRequests.Remove(passwordResetRequest);
                await context.SaveChangesAsync();
                
                return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                    "Reset token has expired. Please request a new password reset.");
            }
            
            if (passwordResetWithTokenDto.NewPassword != passwordResetWithTokenDto.ConfirmPassword)
                return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                    "Passwords do not match.");

            var user = await context.Users
                .Where(u => u.Email == passwordResetRequest.Email)
                .FirstOrDefaultAsync();
                
            if (user == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "User account not found.");
            
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordResetWithTokenDto.NewPassword, salt);

            user.PasswordHash = passwordHash;
            
            context.PasswordResetRequests.Remove(passwordResetRequest);
            
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Password reset successful.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> ValidateResetTokenAsync(ValidateResetTokenDto validateResetTokenDto)
    {
        try
        {
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Email == validateResetTokenDto.Email)
                .FirstOrDefaultAsync();
        
            if (passwordResetRequest == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "A password reset request for this account does not exist. Try creating a new one.");

            if (passwordResetRequest.Token != validateResetTokenDto.Token)
                return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                    "Invalid reset code.");
            
            context.PasswordResetRequests.Remove(passwordResetRequest);
            await context.SaveChangesAsync();
                
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value, 
                "Reset code validated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> ResetPasswordAsync(PasswordResetDto passwordResetDto)
    {
        try
        {
            if (passwordResetDto.NewPassword != passwordResetDto.ConfirmPassword)
                return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                    "Passwords have to match.");
        
            var user = await context.Users
                .Where(u => u.Email == passwordResetDto.Email)
                .FirstOrDefaultAsync();
        
            if (user == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    $"Account tied to email {passwordResetDto.Email} does not exist.");
        
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordResetDto.NewPassword, salt);

            user.PasswordHash = passwordHash;
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value, 
                "Password was reset successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> DeletePasswordResetRequestAsync(int passwordResetRequestId)
    {
        try
        {
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Id == passwordResetRequestId)
                .FirstOrDefaultAsync();
            
            if (passwordResetRequest == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Password reset request not found");

            context.PasswordResetRequests.Remove(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.NoContent,
                Unit.Value,
                "Password reset request deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}