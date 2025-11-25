
namespace Feed.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
