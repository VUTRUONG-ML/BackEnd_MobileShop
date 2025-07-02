using Microsoft.EntityFrameworkCore;

namespace BackEnd_MobileShop.Models
{
    public class MobileShopDbContext : DbContext
    {
        public MobileShopDbContext(DbContextOptions<MobileShopDbContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Model> Models { get; set; } = null!;
        public DbSet<Mobile> Mobiles { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
