using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using equilog_backend.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace equilog_backend.Services;

public class AuthService(EquilogDbContext context, JwtSettings jwtSettings, IMapper mapper) : IAuthService
{
    public string GenerateToken(User user)
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
            SigningCredentials = new SigningCredentials( // Specifies how the JWT signature should be composed.
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha512Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<ApiResponse<AuthResponseDto?>> Register(RegisterDto registerDto)
    {
        try
        {
            var existingUserByUsername = await context.Users
                .FirstOrDefaultAsync(u => u.UserName.Equals(registerDto.UserName,
                    StringComparison.CurrentCultureIgnoreCase));

            if (existingUserByUsername != null)
            {
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.BadRequest,
                    "Username already exists");
            }

            var existingUserByEmail = await context.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(registerDto.Email,
                    StringComparison.CurrentCultureIgnoreCase));

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
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                PasswordHash = passwordHash,
                Salt = salt
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            
            var token = GenerateToken(user);
        
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

    public async Task<ApiResponse<AuthResponseDto?>> Login(LoginDto loginDto)
    {
        try
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserName.Equals(loginDto.UserName,
                    StringComparison.CurrentCultureIgnoreCase));

            if (user == null)
                return ApiResponse<AuthResponseDto?>.Failure(
                    HttpStatusCode.Unauthorized, 
                    "Invalid username or password");
            
            var isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            
            if(!isValidPassword)
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.Unauthorized, 
                    "Invalid username or password");

            var token = GenerateToken(user);
            
            var response = new AuthResponseDto
            {
                Token = token,
            };
            
            return ApiResponse<AuthResponseDto>.Success(
                HttpStatusCode.OK,
                response,
                null); 
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}