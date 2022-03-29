using FirmenpartnerBackend.Models.Data;

namespace FirmenpartnerBackend.Models.Response
{
    public class AuthResponse : AuthResult
    {
        public Guid? UserId { get; set; }
    }
}
