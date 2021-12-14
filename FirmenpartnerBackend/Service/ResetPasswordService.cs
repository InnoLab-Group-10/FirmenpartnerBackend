using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;

namespace FirmenpartnerBackend.Service
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;

        public ResetPasswordService(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }

        public async Task<ChangePasswordResponse> RequestPasswordReset(ApplicationUser user)
        {
            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            try
            {
                await emailService.SendAsync(user.Email, "FirmenpartnerDB Password Reset", $"Password Reset Token:\n{token}");
            }
            catch (Exception e)
            {
                return new ChangePasswordResponse()
                {
                    Success = false,
                    Errors = new List<string>() {
                        e.Message
                    }
                };
            }

            return new ChangePasswordResponse()
            {
                Success = true,
                Errors = new List<string>()
            };
        }

        public async Task<ChangePasswordResponse> FinalizePasswordReset(ApplicationUser user, string token, string newPassword)
        {
            IdentityResult result = await userManager.ResetPasswordAsync(user, token, newPassword);

            ChangePasswordResponse response;
            if (result.Succeeded)
            {
                response = new ChangePasswordResponse()
                {
                    Success = true,
                    Errors = new List<string>()
                };
            }
            else
            {
                response = new ChangePasswordResponse()
                {
                    Success = false,
                    Errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList()
                };
            }

            return response;
        }
    }
}
