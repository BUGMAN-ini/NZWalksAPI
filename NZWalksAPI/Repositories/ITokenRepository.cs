using Microsoft.AspNetCore.Identity;

namespace NZWalksAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser identityUser, List<string> Roles);
    }
}
