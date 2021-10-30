using JamboPayRewards.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JamboPayRewards
{
    /// <summary>
    /// Database Context class
    /// </summary>
    public class DbContext: IdentityDbContext<User>
    {
        public DbContext(DbContextOptions<DbContext> options): base(options){}
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<AmbassadorSupporter> AmbassadorSupporters { get; set; }
        public DbSet<Utility> Utilities { get; set; }
    }
}