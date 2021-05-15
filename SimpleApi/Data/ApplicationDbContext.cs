using Microsoft.EntityFrameworkCore;

using SimpleApi.Model;
using SimpleApi.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Contact>(entity => 
            {
                entity.Property(e => e.InsertDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Contact>().HasOne(e => e.Company).WithMany(e => e.Contacts)
                .HasForeignKey(e => e.CompanyId);
        }
    }
}
