using DbLab2.DataModels;
using Microsoft.EntityFrameworkCore;


namespace DbLab2.DbBookContext
{
    public class BookstoreContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StockBalance> StockBalance { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.ISBN13);
            modelBuilder.Entity<StockBalance>().HasKey(sb => new { sb.StoreID, sb.ISBN });
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderID, od.ISBN });

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorID);

            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Books)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.PublisherID);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.StockBalance)
                .WithOne(sb => sb.Store)
                .HasForeignKey(sb => sb.StoreID);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.StockBalance)
                .WithOne(sb => sb.Book)
                .HasForeignKey(sb => sb.ISBN);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerID);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderID);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.OrderDetails)
                .WithOne(od => od.Book)
                .HasForeignKey(od => od.ISBN);

            modelBuilder.Entity<StockBalance>()
                .HasOne(sb => sb.Store)
                .WithMany(s => s.StockBalance)
                .HasForeignKey(sb => sb.StoreID);

            modelBuilder.Entity<StockBalance>()
                .HasOne(sb => sb.Book)
                .WithMany(b => b.StockBalance)
                .HasForeignKey(sb => sb.ISBN);

        }
    }
}
