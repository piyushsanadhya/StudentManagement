using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Student> Students { get; set; }

        public int SaveChanges(string username)
        {
            AddAuditInfo(username);

            return SaveChanges();
        }

        public int SaveChanges(string username,bool acceptAllChangesOnSuccess)
        {
            AddAuditInfo(username);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public Task<int> SaveChangesAsync(string username,CancellationToken cancellationToken = default(CancellationToken))
        {
            AddAuditInfo(username);
            return base.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(string username,bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddAuditInfo(username);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }

        private void AddAuditInfo(string username)
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditableEntity>()
                                                    .Where(p => p.State == EntityState.Added)
                                                    .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditableEntity>()
                                                       .Where(p => p.State == EntityState.Modified)
                                                       .Select(p => p.Entity);

            var utcNow = DateTime.UtcNow;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedOnUtc = utcNow;
                added.CreatedBy = username;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.LastModifiedOnUtc = utcNow;
                modified.LastModifiedBy = username;
            }
        }
    }
}
