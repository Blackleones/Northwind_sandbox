using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Northwind.EF.Persistence.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddNorthwindContext(this IServiceCollection services)
        {
            services.AddDbContext<NorthwindDbContext>(opt => 
                opt.EnableSensitiveDataLogging()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;"));
        }
    }
}