public interface ITokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
