using VueZtmBackend.Domain.Entities;

namespace VueZtmBackend.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid? ValidateToken(string token);
}

