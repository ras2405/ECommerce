using Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccess.Context
{
    public class ECommerceContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<PurchasedProduct> PurchasedProducts { get; set; }

        public ECommerceContext() {}

        public ECommerceContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
