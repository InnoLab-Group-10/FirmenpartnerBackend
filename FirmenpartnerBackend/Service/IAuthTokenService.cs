using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;

namespace FirmenpartnerBackend.Service
{
    public interface IAuthTokenService
    {
        Task<AuthResult> GenerateToken(ApplicationUser user);
        Task<bool> RemoveRefreshToken(string token);
        Task<AuthResult> RefreshToken(TokenRequest tokenRequest);
    }
}