using Guide_Project.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guide_Project.Data;

public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CommercialActivity> CommercialActivities { get; set; }
    public DbSet<ReportFile> ReportFiles { get; set; }
    //public DbSet<RefreshToken> RefreshTokens { get; set; }
    /* protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CommercialActivity>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.CommercialActivities)
                .HasForeignKey(x => x.CustomerId);
    } */
}