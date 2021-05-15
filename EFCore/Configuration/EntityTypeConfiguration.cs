using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLaucher.Configuration
{
    class EntityTypeConfiguration
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

        #region Configuration

        public class PersonConfiguration : IEntityTypeConfiguration<Person>
        {
            public void Configure(EntityTypeBuilder<Person> builder)
            {
                // Map to Person_Table
                builder.ToTable("Person_Table");

                // Configure PrimaryKey
                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id).ValueGeneratedOnAdd();

                // Map to Person_Table.Person_Name
                builder.Property(e => e.Name).HasColumnName("Person_Name");

                // Set max column lenght
                builder.Property(e => e.Surname).HasMaxLength(15);

                // Set not null column
                builder.Property(e => e.Email).IsRequired();

                // Field not mapped with the database
                builder.Ignore(e => e.Profession);
            }
        }

        #endregion

        #region Context

        public class TypeConfigurationContextFactory : IDesignTimeDbContextFactory<TypeConfigurationContext>
        {
            public TypeConfigurationContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<TypeConfigurationContext>();
                optionsBuilder.UseSqlite("Filename = TypeConfigurationContext.db");

                return new TypeConfigurationContext(optionsBuilder.Options);
            }
        }

        public class TypeConfigurationContext : DbContext
        {
            public TypeConfigurationContext(DbContextOptions<TypeConfigurationContext> options) : base() { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = TypeConfigurationContext.db");
            }
            public DbSet<Person> Persons { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfiguration(new PersonConfiguration());
            }
        }

        #endregion

        #region Migration

        public partial class TypeConfigurationMigration : Migration
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
