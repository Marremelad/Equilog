using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Models;

namespace equilog_backend.Services;

public class PasswordResetService(EquilogDbContext context)
{
    public async Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequest(int id)
    {
        try
        {
            var passwordResetRequest = new PasswordResetRequest()
            {
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}