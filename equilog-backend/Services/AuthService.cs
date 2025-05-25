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
    // Creates a JWT token for the authenticated user.
    private string CreateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
        
        // Configure the token with user claims and expiration.
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
        
        // Generate and return the JWT token as a string.
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    // Registers a new user and returns authentication tokens.
    public async Task<ApiResponse<AuthResponseDto?>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Check if a user with this email already exists.
            var existingUserByEmail = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());

            if (existingUserByEmail != null)
            {
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.BadRequest,
                    "Email already exists.");
            }
            
            // Hash the password using BCrypt with generated salt.
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, salt);

            // Create a new user entity and set the hashed password.
            var user = mapper.Map<User>(registerDto);
            user.PasswordHash = passwordHash;

            // Save the new user to the database.
            context.Users.Add(user);
            await context.SaveChangesAsync();
            
            // Generate authentication tokens for the new user.
            var accessToken = CreateJwt(user);
            var refreshToken = await CreateRefreshTokenAsync(user.Id);
            
            // Return a successful response with tokens.
            var response = new AuthResponseDto
            {
                AccessToken = accessToken, // JWT.
                RefreshToken = refreshToken.Token,
            };
            
            return ApiResponse<AuthResponseDto>.Success(
                HttpStatusCode.Created,
                response,
                "User registered successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Authenticates user credentials and returns tokens if valid.
    public async Task<ApiResponse<AuthResponseDto?>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Find a user by email (case-insensitive).
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null)
                return ApiResponse<AuthResponseDto?>.Failure(
                    HttpStatusCode.Unauthorized, 
                    "Invalid email or password.");
            
            // Verify the provided password against stored hash.
            var isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            
            if(!isValidPassword)
                return ApiResponse<AuthResponseDto>.Failure(
                    HttpStatusCode.Unauthorized, 
                    "Invalid email or password.");

            // Generate new authentication tokens for successful login.
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
                "Login successful."); 
        }
        catch (Exception ex)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Validates if the provided password matches the user's stored password.
    public async Task<ApiResponse<Unit>> ValidatePasswordAsync(ValidatePasswordDto validatePasswordDto)
    {
        try
        {
            // Find the user by ID.
            var user = await context.Users
                .Where(u => u.Id == validatePasswordDto.UserId)
                .FirstOrDefaultAsync();
            
            if (user == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound, 
                    "Error: User not found.");
            
            // Check if the provided password matches the stored hash.
            if (BCrypt.Net.BCrypt.Verify(validatePasswordDto.Password, user.PasswordHash))
                return ApiResponse<Unit>.Success(
                    HttpStatusCode.OK,
                    Unit.Value,
                    "Password verified successfully.");
            
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.BadRequest,
                "Incorrect password.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new refresh token for the specified user.
    private async Task<RefreshToken> CreateRefreshTokenAsync(int userId)
    {
        var token = Guid.NewGuid().ToString();
    
        // Create a refresh token with 7-day expiration.
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserIdFk = userId,
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            IsUsed = false
        };
    
        // Save the refresh token to the database.
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
    
        return refreshToken;
    }
    
    // Validates refresh token by checking expiration, usage, and revocation status.
    private bool ValidateRefreshToken(RefreshToken? token)
    {
        if (token == null)
            return false;
        
        // Check if the token has expired.
        if (token.ExpirationDate <= DateTime.UtcNow)
            return false;
        
        // Check if the token has already been used.
        if (token.IsUsed)
            return false;
        
        // Check if the token has been revoked.
        if (token.IsRevoked)
            return false;
        
        return true;
    }
    
    // Uses refresh token to generate new access and refresh tokens.
    public async Task<ApiResponse<AuthResponseDto?>> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            // Find the refresh token in the database with the associated user.
            var storedRefreshToken = await context.RefreshTokens
                .Include(rt => rt.User)
                .Where(rt => rt.Token == refreshToken)
                .FirstOrDefaultAsync();
            
            // Validate the refresh token.
            if (storedRefreshToken == null || !ValidateRefreshToken(storedRefreshToken))
            {
                return ApiResponse<AuthResponseDto?>.Failure(
                    HttpStatusCode.BadRequest, 
                    "Invalid refresh token.");
            }

            // Mark the current refresh token as used.
            storedRefreshToken.IsUsed = true;
            context.RefreshTokens.Update(storedRefreshToken);
        
            var user = storedRefreshToken.User!;
        
            // Generate new tokens for the user.
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
    
    // Revokes a refresh token to prevent further use.
    public async Task<ApiResponse<Unit>> RevokeRefreshTokenAsync(string refreshToken)
    {
        try
        {
            // Find the refresh token in the database.
            var storedRefreshToken = await context.RefreshTokens
                .Where(rt => rt.Token == refreshToken)
                .FirstOrDefaultAsync();

            if (storedRefreshToken == null)
            {
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Invalid refresh token.");
            }

            // Mark the token as revoked.
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