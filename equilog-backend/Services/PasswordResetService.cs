using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;

namespace equilog_backend.Services;

public class PasswordResetService(EquilogDbContext context, IMapper mapper) : IPasswordResetService
{
    public async Task<ApiResponse<PasswordResetDto?>> CreatePasswordResetRequest(int id)
    {
        try
        {
            var passwordResetRequest = new PasswordResetRequest()
            {
                UserIdFk = id,
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
}