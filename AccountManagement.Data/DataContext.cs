using AccountManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<AccountModel> AccountModels { get; set; }
    }
}