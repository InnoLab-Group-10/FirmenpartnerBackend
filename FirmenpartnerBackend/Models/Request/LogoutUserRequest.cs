using System.ComponentModel.DataAnnotations;

namespace FirmenpartnerBackend.Models.Request
{
    public class LogoutUserRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
