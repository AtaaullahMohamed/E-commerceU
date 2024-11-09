using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {


      
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {    // Define your model relationships or configurations here
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductCofiguration).Assembly);
        }

    }
}
