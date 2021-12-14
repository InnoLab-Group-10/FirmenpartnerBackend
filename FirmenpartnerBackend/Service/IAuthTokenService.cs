using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace FirmenpartnerBackend.Service
{
    public interface IAuthTokenService
    {
        Task<AuthResult> GenerateToken(IdentityUser user);
        Task<bool> RemoveRefreshToken(string token);
        Task<AuthResult> RefreshToken(TokenRequest tokenRequest);
    }
}