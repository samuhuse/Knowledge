using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLaucher.Configuration.DataAnnotation
{
    public class SimpleDataAnnotation
    {
        #region Model

        [Table("Person_Table")] // Map to Person_Table
        public class Person
        {
            [Key] // PrimaryKey
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Set value generation on identity
            //[DatabaseGenerated(DatabaseGeneratedOption.None)] // Set off any value generation
            //[DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Set value generation on computed, set last max value = 1 every time the record is updated 
            public int Id { get; set; }
            [Column("Person_Name")] // Map to Person_Table.Person_Name
            public string Name { get; set; }
            [MaxLength(15)] // Set max column lenght
            public string Surname { get; set; }
            [Required] // Set not null column
            public string Email { get; set; }

            [NotMapped] // Field not mapped with the database
            public string Profession { get; set; }
        }

        #endregion

        #region Context

        public class SimpleDataAnnotationContextFactory : IDesignTimeDbContextFactory<SimpleDataAnnotationContext>
        {
            public SimpleDataAnnotationContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<SimpleDataAnnotationContext>();
                optionsBuilder.UseSqlite("Filename = SimpleDataAnnotation.db");

                return new SimpleDataAnnotationContext(optionsBuilder.Options);
            }
        }

        public class SimpleDataAnnotationContext : DbContext
        {
            public SimpleDataAnnotationContext(DbContextOptions<SimpleDataAnnotationContext> options) : base() { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = SimpleDataAnnotation.db");
            }

            public DbSet<Person> Persons { get; set; }
        }

        #endregion


        #region SqliteMigration

        public partial class SimpleDataAnnotationMigration : Migration
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
