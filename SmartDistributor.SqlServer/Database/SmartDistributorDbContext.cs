using Microsoft.EntityFrameworkCore;
using SmartDistributor.Models.Main;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.SqlServer.Database
{
    public class SmartDistributorDbContext : DbContext
    {
        public SmartDistributorDbContext(DbContextOptions<SmartDistributorDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Geolocation> Geolocations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<State> States { get; set; }
    }
}
