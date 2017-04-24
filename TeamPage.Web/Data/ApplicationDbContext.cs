using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamPage.Web.Models;

namespace TeamPage.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agency> Agencies { get; set; }

        public DbSet<AgencyClientAssignment> AgencyClientAssignments { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<UserAgencyAssignment> UserAgencyAssignments { get; set; }

        public DbSet<UserAgencyAssignment> UserClientAssignments { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Agency>()
                .HasIndex(b => b.UniqueCode)
                .IsUnique();

            builder.Entity<User>()
                .HasMany(u => u.AgencyAssignments)
                .WithOne(x => x.User)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.UserId);

            builder.Entity<User>()
                .HasMany(u => u.ClientAssignments)
                .WithOne(x => x.User)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.UserId);

            builder.Entity<AgencyClientAssignment>()
               .HasKey(a => new { a.ClientId, a.AgencyId });


            builder.Entity<UserAgencyAssignment>()
               .HasKey(a => new { a.UserId, a.AgencyId });
            builder.Entity<UserAgencyAssignment>()
                .HasOne(u => u.User)
                .WithMany(x => x.AgencyAssignments)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.UserId);
            builder.Entity<UserAgencyAssignment>()
                .HasOne(u => u.Agency)
                .WithMany(x => x.UserAssignments)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.AgencyId);

            builder.Entity<UserClientAssignment>()
               .HasKey(a => new { a.UserId, a.ClientId });
            builder.Entity<UserClientAssignment>()
                .HasOne(u => u.User)
                .WithMany(x => x.ClientAssignments)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.UserId);
            builder.Entity<UserClientAssignment>()
                .HasOne(u => u.Client)
                .WithMany(x => x.UserAssignments)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.ClientId);

        }
    }
}
