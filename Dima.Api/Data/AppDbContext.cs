using System.Reflection;
using Dima.Api.Data.Mappings;
using Dima.Api.Models;
using Dima.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Data;

public class AppDbContext : IdentityDbContext<
    User, 
    IdentityRole<long>,
    long,
    IdentityUserClaim<long>,
    IdentityUserRole<long>,
    IdentityUserLogin<long>,
    IdentityRoleClaim<long>,
    IdentityUserToken<long>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
    }
}