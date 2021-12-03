using FirmenpartnerBackend.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirmenpartnerBackend.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

    }
}
