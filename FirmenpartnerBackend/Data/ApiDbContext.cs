using FirmenpartnerBackend.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirmenpartnerBackend.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyLocation> CompanyLocations { get; set; }
        public DbSet<Models.Data.Program> Programs { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CompanyAssignment> CompanyAssignments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TimelineEntry> TimelineEntries { get; set; }
        public DbSet<FileEntry> FileEntries { get; set; }


        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
