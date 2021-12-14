using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;

namespace FirmenpartnerBackend.Service
{
    public interface IResetPasswordService
    {
        Task<ChangePasswordResponse> FinalizePasswordReset(ApplicationUser user, string token, string newPassword);
        Task<ChangePasswordResponse> RequestPasswordReset(ApplicationUser user);
    }
}