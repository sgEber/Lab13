using Microsoft.EntityFrameworkCore;
namespace Lab13.Models
{
    public class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=EBER\\SQLEXPRESS;" +
                "Initial Catalog=StoreDB; Integrated Security=True; trustservercertificate=True");
        }
    }
}
