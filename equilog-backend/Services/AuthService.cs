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
using Twilio.Rest.Trunking.V1;

namespace equilog_backend.Services;

public class AuthService(EquilogDbContext context, JwtSettings jwtSettings, IMapper mapper) : IAuthService
{
    public string CreateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
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

            var user = mapper.Map<User>(registerDto);
            user.PasswordHash = passwordHash;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            
            var accessToken = CreateJwt(user);
            var refreshToken = await CreateRefreshTokenAsync(user.Id);
            
            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
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

            var accessToken = CreateJwt(user);
            var refreshToken = await CreateRefreshTokenAsync(user.Id);
            
            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
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
    
    public async Task<ApiResponse<Unit>> ValidatePasswordAsync(ValidatePasswordDto validatePasswordDto)
    {
        try
        {
            var user = await context.Users
                .Where(u => u.Id == validatePasswordDto.UserId)
                .FirstOrDefaultAsync();
            
            if (user == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found.");
            
            if (BCrypt.Net.BCrypt.Verify(validatePasswordDto.Password, user.PasswordHash))
                return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                    Unit.Value,
                    null);
            
            return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                "Incorrect password.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<RefreshToken> CreateRefreshTokenAsync(int userId)
    {
        var token = Guid.NewGuid().ToString();
    
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserIdFk = userId,
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            IsUsed = false
        };
    
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
    
        return refreshToken;
    }
    
    public bool ValidateRefreshToken(RefreshToken? token)
    {
        if (token == null)
            return false;
        
        if (token.ExpirationDate <= DateTime.UtcNow)
            return false;
        
        if (token.IsUsed)
            return false;
        
        if (token.IsRevoked)
            return false;
        
        return true;
    }
    
    public async Task<ApiResponse<AuthResponseDto?>> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var storedRefreshToken = await context.RefreshTokens
                .Include(rt => rt.User)
                .Where(rt => rt.Token == refreshToken)
                .FirstOrDefaultAsync();

            if (storedRefreshToken == null || !ValidateRefreshToken(storedRefreshToken))
            {
                return ApiResponse<AuthResponseDto?>.Failure(
                    HttpStatusCode.BadRequest, 
                    "Invalid refresh token.");
            }

            storedRefreshToken.IsUsed = true;
            context.RefreshTokens.Update(storedRefreshToken);
        
            var user = storedRefreshToken.User!;
        
            var newRefreshToken = await CreateRefreshTokenAsync(user.Id);
        
            var newAccessToken = CreateJwt(user);
        
            await context.SaveChangesAsync();
        
            var response = new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
            };
        
            return ApiResponse<AuthResponseDto?>.Success(
                HttpStatusCode.OK,
                response,
                "Token refreshed successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto?>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> RevokeRefreshTokenAsync(string refreshToken)
    {
        try
        {
            var storedRefreshToken = await context.RefreshTokens
                .Where(rt => rt.Token == refreshToken)
                .FirstOrDefaultAsync();

            if (storedRefreshToken == null)
            {
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Invalid refresh token.");
            }

            storedRefreshToken.IsRevoked = true;
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Token successfully revoked.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}