using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace FirmenpartnerBackend.Service
{
    public interface IResetPasswordService
    {
        Task<ChangePasswordResponse> FinalizePasswordReset(IdentityUser user, string token, string newPassword);
        Task<ChangePasswordResponse> RequestPasswordReset(IdentityUser user);
    }
}