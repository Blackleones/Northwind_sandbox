using Northwind.EF.Persistence;
using System;

namespace Northwind
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new NorthwindDbContextFactory();

            using (var context = factory.CreateDbContext(null)) 
            {
                NorthwindInitializer.Initialize(context);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
