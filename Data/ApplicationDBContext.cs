using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using dotnet_api_learning.Models;

namespace dotnet_api_learning.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        // this allows to delete stock and all its comments in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d", 
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new {p.AppUserId, p.StockId}));

            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId); 

            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId); 
        }
        
    }
}