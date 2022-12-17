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
        public DbSet<MailingListEntry> MailingListsEntries { get; set; }
        public DbSet<MailingList> MailingLists { get; set; }
        public DbSet<MailSetting> MailSettings { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }


        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Company>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<CompanyLocation>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<Models.Data.Program>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<Person>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<CompanyAssignment>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<TimelineEntry>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<FileEntry>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<MailingListEntry>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<MailingList>().HasQueryFilter(entity => !entity.IsDeleted);
            builder.Entity<MailTemplate>().HasQueryFilter(entity => !entity.IsDeleted);

            base.OnModelCreating(builder);
        }

        // "inspired" by https://www.ryansouthgate.com/2019/01/07/entity-framework-core-soft-delete/
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is ISoftDeletable entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;

                    // Set the IsDeleted property to true and set the deletion time to the current time
                    entity.IsDeleted = true;
                    entity.DeletedTimestamp = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is ISoftDeletable entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;

                    // Set the IsDeleted property to true and set the deletion time to the current time
                    entity.IsDeleted = true;
                    entity.DeletedTimestamp = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is ISoftDeletable entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;

                    // Set the IsDeleted property to true and set the deletion time to the current time
                    entity.IsDeleted = true;
                    entity.DeletedTimestamp = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
