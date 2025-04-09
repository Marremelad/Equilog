using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Security;

namespace equilog_backend.Services;

public class AuthService(EquilogDbContext context, JwtSettings jwtSettings) : IAuthService
{
    
}