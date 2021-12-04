using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
