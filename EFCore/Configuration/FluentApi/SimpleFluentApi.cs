using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLaucher.Configuration.FluentApi
{
    public class SimpleFluentApi
    {
        #region Model

        public class Person // Map to Person_Table
        {
            public int Id { get; set; } // Key, Identity
            public string Name { get; set; } // Map to Person_Table.Person_Name
            public string Surname { get; set; } // Set max column lenght
            public string Email { get; set; } // Set not null column
            public string Profession { get; set; } // Field not mapped with the database
        }

        #endregion

        #region Context

        public class SimpleFluentContextFactory : IDesignTimeDbContextFactory<SimpleFluentContext>
        {
            public SimpleFluentContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<SimpleFluentContext>();
                optionsBuilder.UseSqlite("Filename = SimpleFluentContext.db");

                return new SimpleFluentContext(optionsBuilder.Options);
            }
        }

        public class SimpleFluentContext : DbContext
        {
            public SimpleFluentContext(DbContextOptions<SimpleFluentContext> options) : base() { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = SimpleFluentContext.db");
            }
            public DbSet<Person> Persons { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Map to Person_Table
                modelBuilder.Entity<Person>().ToTable("Person_Table");

                // Configure PrimaryKey
                modelBuilder.Entity<Person>().HasKey(e => e.Id);
                modelBuilder.Entity<Person>().Property(e => e.Id).ValueGeneratedOnAdd();

                // Map to Person_Table.Person_Name
                modelBuilder.Entity<Person>().Property(e => e.Name).HasColumnName("Person_Name");

                // Set max column lenght
                modelBuilder.Entity<Person>().Property(e => e.Surname).HasMaxLength(15);

                // Set not null column
                modelBuilder.Entity<Person>().Property(e => e.Email).IsRequired();

                // Field not mapped with the database
                modelBuilder.Entity<Person>().Ignore(e => e.Profession);
            }
        }

        #endregion

        #region Migration

        public partial class SimpleFluent : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.CreateTable(
                    name: "Person_Table",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Person_Name = table.Column<string>(type: "TEXT", nullable: true),
                        Surname = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                        Email = table.Column<string>(type: "TEXT", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Person_Table", x => x.Id);
                    });
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropTable(
                    name: "Person_Table");
            }
        }

        #endregion

    }
}
