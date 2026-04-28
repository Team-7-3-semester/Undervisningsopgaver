using Undervisningsopgaver.Models;

namespace Undervisningsopgaver.Services;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
