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
    public async Task<ApiResponse<PasswordResetDto?>> CreatePasswordResetRequestAsync(string email)
    {
        try
        {
            if (!await context.Users
                    .AnyAsync(u => u.Email == email))
                return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.BadRequest,
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
            
            return ApiResponse<PasswordResetDto?>.Success(HttpStatusCode.Created,
                mapper.Map<PasswordResetDto>(passwordResetRequest),
                "Password reset request was created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<PasswordResetDto?>> DeletePasswordResetRequestAsync(int id)
    {
        try
        {
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Id == id)
                .FirstOrDefaultAsync();
            
            if (passwordResetRequest == null)
                return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.NotFound,
                    "Error: Password reset request not found");

            context.PasswordResetRequests.Remove(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<PasswordResetDto?>.Success(HttpStatusCode.NoContent,
                null,
                "Password reset request deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetDto?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}