using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using equilog_backend.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace equilog_backend.Services;

public class AuthService(EquilogDbContext context, JwtSettings jwtSettings) : IAuthService
{
    public string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ]),
            Expires = DateTime.Now.AddMinutes(jwtSettings.DurationInMinutes),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha512Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<ApiResponse<AuthResponseDto?>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var existingUserByUsername = await context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == registerDto.UserName.ToLower());

            if (existingUserByUsername != null)
            {
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.BadRequest,
                    "Username already exists");
            }

            var existingUserByEmail = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());

            if (existingUserByEmail != null)
            {
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.BadRequest,
                    "Email already exists");
            }
            
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, salt);

            var user = new User
            {
                UserName = registerDto.UserName,
                PasswordHash = passwordHash,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            
            var token = GenerateJwt(user);
        
            var response = new AuthResponseDto
            {
                Token = token,
            };
            
            return ApiResponse<AuthResponseDto>.Success(
                HttpStatusCode.Created,
                response,
                "User registered successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<AuthResponseDto?>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null)
                return ApiResponse<AuthResponseDto?>.Failure(
                    HttpStatusCode.Unauthorized, 
                    "Invalid email or password");
            
            var isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            
            if(!isValidPassword)
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.Unauthorized, 
                    "Invalid email or password");

            var token = GenerateJwt(user);
            
            var response = new AuthResponseDto
            {
                Token = token,
            };
            
            return ApiResponse<AuthResponseDto>.Success(
                HttpStatusCode.OK,
                response,
                "Login successful"); 
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}