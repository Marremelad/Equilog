using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that handles password-related operations including reset requests, password changes, and resets.
// Manages secure password reset tokens and password hashing using BCrypt.
public class PasswordService(EquilogDbContext context, IMapper mapper) : IPasswordService
{
    // Creates a password reset request with a unique token for a given email address.
    public async Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email)
    {
        try
        {
            // Verify that a user with the provided email exists.
            if (!await context.Users
                    .AnyAsync(u => u.Email == email))
                return ApiResponse<PasswordResetRequestDto>.Failure(
                    HttpStatusCode.BadRequest,
                    $"Account with the email {email} does not exist.");
            
            // Remove any existing password reset request for this email.
            var storedPasswordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Email == email)
                .FirstOrDefaultAsync();

            if (storedPasswordResetRequest != null)
            {
                context.PasswordResetRequests.Remove(storedPasswordResetRequest);
                await context.SaveChangesAsync();
            }

            // Create a new password reset request with a token and 24-hour expiration.
            var passwordResetRequest = new PasswordResetRequest()
            {
                Email = email,
                Token = Generate.PasswordResetToken(),
                ExpirationDate = DateTime.UtcNow.AddHours(24)
            };

            // Save the password reset request to database.
            context.PasswordResetRequests.Add(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<PasswordResetRequestDto>.Success(
                HttpStatusCode.Created,
                mapper.Map<PasswordResetRequestDto>(passwordResetRequest),
                "Password reset request was created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<PasswordResetRequestDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Resets a user's password using a valid reset token.
    public async Task<ApiResponse<Unit>> ResetPasswordAsync(PasswordResetDto passwordResetDto)
    {
        try
        {
            // Find the password reset request by token.
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Token == passwordResetDto.Token)
                .FirstOrDefaultAsync();
            
            // Returns an error if token is invalid.
            if (passwordResetRequest == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Invalid reset token.");
            
            // Check if the token has expired and clean up if necessary.
            if (passwordResetRequest.ExpirationDate < DateTime.UtcNow)
            {
                context.PasswordResetRequests.Remove(passwordResetRequest);
                await context.SaveChangesAsync();
                
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "Reset token has expired. Please request a new password reset.");
            }
            
            // Validate that password and confirmation match.
            if (passwordResetDto.NewPassword != passwordResetDto.ConfirmPassword)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "Passwords do not match.");

            // Find the user associated with the reset request.
            var user = await context.Users
                .Where(u => u.Email == passwordResetRequest.Email)
                .FirstOrDefaultAsync();
                
            if (user == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "User account not found.");
            
            // Hash the new password using BCrypt with generated salt.
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordResetDto.NewPassword, salt);

            // Update a user's password hash and remove the used reset request.
            user.PasswordHash = passwordHash;
            context.PasswordResetRequests.Remove(passwordResetRequest);
            
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Password reset successful.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Changes a user's password directly in the application (for authenticated users).
    public async Task<ApiResponse<Unit>> ChangePasswordAsync(PasswordChangeDto passwordChangeDto)
    {
        try
        {
            // Find the user by ID.
            var user = await context.Users
                .Where(u => u.Id == passwordChangeDto.UserId)
                .FirstOrDefaultAsync();
            
            if (user == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not found.");
            
            // Validate that password and confirmation match.
            if (passwordChangeDto.NewPassword != passwordChangeDto.ConfirmPassword)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "Passwords have to match.");
            
            // Hash the new password using BCrypt with generated salt.
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordChangeDto.NewPassword, salt);

            // Update the user's password hash.
            user.PasswordHash = passwordHash;
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value, 
                "Password was reset successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Removes a password reset request from the system (cleanup operation).
    public async Task<ApiResponse<Unit>> DeletePasswordResetRequestAsync(int passwordResetRequestId)
    {
        try
        {
            // Find the password reset request to delete.
            var passwordResetRequest = await context.PasswordResetRequests
                .Where(prr => prr.Id == passwordResetRequestId)
                .FirstOrDefaultAsync();
            
            if (passwordResetRequest == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Password reset request not found.");

            // Remove the password reset request from the database.
            context.PasswordResetRequests.Remove(passwordResetRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Password reset request deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}