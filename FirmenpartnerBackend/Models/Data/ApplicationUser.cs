using Microsoft.AspNetCore.Identity;

namespace FirmenpartnerBackend.Models.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? Notes { get; set; }
    }
}
