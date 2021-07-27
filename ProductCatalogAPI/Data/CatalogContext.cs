using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Domain
{
    public class CatalogContext : DbContext //provide instructions to EntityFramework
    {
        //giving dependency injection(location), injecting to part
        public CatalogContext(DbContextOptions options) : base(options) //DI: a parameter accepted in a constructor of class; injecting location where database can be found; passing the connection string(location) to the base class
        {

        }

        //defining which classes should be converted into tables

        public DbSet<CatalogType> CatalogTypes { get; set; } //create table of CatalogType
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogItem> Catalog { get; set; }

        //giving instructions to modelbulider how the models should be constructed.

        protected override void OnModelCreating(ModelBuilder modelBuilder) //creates models into a table; table = model = entity
        {
            modelBuilder.Entity<CatalogType>(e =>
            {
                e.Property(t => t.Id)
                  .IsRequired()
                  .ValueGeneratedOnAdd(); //everytime you add a new row into the table this value for this column should be automatically generated; i.e Primary Key

                e.Property(t => t.Type)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CatalogBrand>(e =>
            {
                e.Property(b => b.Id)
                  .IsRequired()
                  .ValueGeneratedOnAdd(); //everytime you add a new row into the table this value for this column should be automatically generated; i.e Primary Key

                e.Property(b => b.Brand)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CatalogItem>(e =>
            {
                e.Property(c => c.Id)
                  .IsRequired()
                  .ValueGeneratedOnAdd();

                e.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                e.Property(c => c.Price)
                    .IsRequired();

                e.HasOne(c => c.CatalogType)
                     .WithMany()
                     .HasForeignKey(c => c.CatalogTypeId);

                e.HasOne(c => c.CatalogBrand)
                     .WithMany()
                     .HasForeignKey(c => c.CatalogBrandId);
            });




        }
    }
}
