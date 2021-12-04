using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class FinalizeResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string ResetToken { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
