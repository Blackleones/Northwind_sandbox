using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.EF.Domain.Entities;
using Northwind.EF.Domain.Models;

namespace Northwind.EF.Persistence
{
    public class NorthwindDbContextFactory : IDesignTimeDbContextFactory<NorthwindDbContext>
    {
        public NorthwindDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;");

            return new NorthwindDbContext(optionsBuilder.Options);
        }
    }

    public class NorthwindDbContext : DbContext
    {
        public NorthwindDbContext()
        {
        }

        public NorthwindDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Region> Regions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NorthwindDbContext).Assembly);

            modelBuilder.Entity<CustomersMostPurchasedProducts>()
                .HasNoKey()
                .ToQuery(() => Set<CustomersMostPurchasedProducts>().FromSqlRaw(@"
            select
            	c.CustomerID
            	, c.CompanyName
            	, p.ProductID
            	, p.ProductName
            	, qtyCounts.QuantityPurchased
            	from Customers c
            	inner join
            		(select
            			o.CustomerID
            			, od.ProductID
            			,sum(od.Quantity) as QuantityPurchased
            		from [Order Details] od
            		inner join [Orders] o on od.OrderID = o.OrderID
            		group by o.CustomerID, od.ProductID)  qtyCounts on c.CustomerID = qtyCounts.CustomerID
            	inner join Products p on p.ProductID = qtyCounts.ProductID
            "));
        }
    }
}