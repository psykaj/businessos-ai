using backend.Entities;

namespace backend.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    DateTime GetExpirationDate();
}
