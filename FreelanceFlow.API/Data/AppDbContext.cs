using FreelanceFlow.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreelanceFlow.API.Data
{
    // Inherits from IdentityDbContext<AppUser> instead of plain DbContext,
    // because ASP.NET Identity needs its own tables (Users, Roles, Claims, etc.)
    // and this gives us that for free while still letting us add our own entities.
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User > Client (one freelancer, many clients)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client > Project (one client, many projects)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client > Note (one client, many notes) 
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Client)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project > Task (one project, many tasks)
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project > Invoice (one project, many invoices)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice > Payment (one invoice, many payments)
            modelBuilder.Entity<Payment>()
                .HasOne(pay => pay.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(pay => pay.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // User > RefreshToken (one user, many refresh tokens over time)
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PostgreSQL needs explicit precision/scale for decimal columns,
            // otherwise EF Core defaults to something imprecise for money values.
            modelBuilder.Entity<Project>()
                .Property(p => p.Budget)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProjectTask>()
                .Property(t => t.EstimatedHours)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            // Speeds up the most common lookups: finding all projects for a
            // client, or all invoices for a project — both happen constantly.
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.ClientId);

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.ProjectId);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.InvoiceId);
        }
    }
}