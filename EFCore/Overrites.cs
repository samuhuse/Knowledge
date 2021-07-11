using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    public class Overrites
    {
        #region Model

        public partial class Contact
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }

        #endregion

        #region Context

        public class CrudContext : DbContext
        {
            public CrudContext(DbContextOptions<CrudContext> options) : base(options) { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = Crud.db");
                optionsBuilder.LogTo(Console.WriteLine);
            }
            public DbSet<Contact> Contacts { get; set; }            

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Contact>().HasKey(e => e.Id);
            }
        }

        #endregion

        public partial class Contact
        {
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is Contact contact && contact.Id == this.Id;
            }

            public override int GetHashCode()
            {
                return Id;

                // If clustered key
                return Id ^ Name.GetHashCode(); // Name must be unmodified
            }
        }
    }
}
