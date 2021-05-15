using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLaucher
{
    public class Views
    {
        #region Model

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
        }

        public class PersonFromView
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        #endregion

        #region Context

        public class ViewsContextFactory : IDesignTimeDbContextFactory<ViewsContext>
        {
            public ViewsContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ViewsContext>();
                optionsBuilder.UseSqlite("Filename = Views.db");

                return new ViewsContext(optionsBuilder.Options);
            }
        }

        public class ViewsContext : DbContext
        {
            public ViewsContext(DbContextOptions<ViewsContext> options) : base() { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = Views.db");
            }

            public DbSet<Person> Persons { get; set; }
            public DbSet<PersonFromView> PersonNominatives { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Person>().HasKey(e => e.Id);

                modelBuilder.Entity<PersonFromView>().HasNoKey().ToView("PersonNominatives");
            }

        }

        #endregion

        #region Migration

        public partial class ViewsContextMigration : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.CreateTable(
                    name: "Persons",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Name = table.Column<string>(type: "TEXT", nullable: true),
                        Surname = table.Column<string>(type: "TEXT", nullable: true),
                        Age = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Persons", x => x.Id);
                    });

                migrationBuilder.Sql(@"CREATE VIEW PersonNominatives 
                                        As
                                        Select Name, Surname From Persons
                                        ");
                                        
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropTable(
                    name: "Persons");

                migrationBuilder.Sql("Drop View PersonNominatives");
            }
        }

        #endregion

        ViewsContext _db;

        [SetUp]
        public void StartUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ViewsContext>();
            optionsBuilder.UseSqlite("Filename = Views.db");

            _db = new ViewsContext(optionsBuilder.Options);
        }

        [Test]
        public void ReadFromView()
        {
            var nominatives = _db.PersonNominatives.ToList();
        }
    }
}
