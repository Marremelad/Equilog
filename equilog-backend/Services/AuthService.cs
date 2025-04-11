using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using equilog_backend.Security;
using Microsoft.IdentityModel.Tokens;

namespace equilog_backend.Services;

public class AuthService(EquilogDbContext context, JwtSettings jwtSettings, IMapper mapper) : IAuthService
{
    public string GenerateToke(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
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
    public Task<ApiResponse<AuthResponseDto?>> Register(RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<AuthResponseDto>?> Login(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

   
}