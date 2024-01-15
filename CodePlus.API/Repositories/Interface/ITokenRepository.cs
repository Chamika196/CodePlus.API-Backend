using Microsoft.AspNetCore.Identity;

namespace CodePlus.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
