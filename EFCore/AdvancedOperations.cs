using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreLaucher
{
    public class AdvancedOperations
    {
        #region Model

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Contact Contact { get; set; }
            public Company Company { get; set; }
        }

        public class Contact
        {
            public int Id { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public Person Person { get; set; }
        }

        public class Company
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Person> Persons { get; set; }
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

            public DbSet<Person> Persons { get; set; }
            public DbSet<Contact> Contacts { get; set; }
            public DbSet<Company> Companies { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Contact>().HasKey(e => e.Id);

                modelBuilder.Entity<Person>().HasKey(e => e.Id);
                modelBuilder.Entity<Person>().HasOne(e => e.Contact).WithOne(e => e.Person)
                    .HasForeignKey<Contact>(e => e.Id);

                modelBuilder.Entity<Company>().HasKey(e => e.Id);
                modelBuilder.Entity<Company>().HasMany(e => e.Persons);
            }

        }

        #endregion

        #region Migration

        public partial class CrudContextMigration : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.CreateTable(
                    name: "Companies",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Name = table.Column<string>(type: "TEXT", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Companies", x => x.Id);
                    });

                migrationBuilder.CreateTable(
                    name: "Persons",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Name = table.Column<string>(type: "TEXT", nullable: true),
                        CompanyId = table.Column<int>(type: "INTEGER", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Persons", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Persons_Companies_CompanyId",
                            column: x => x.CompanyId,
                            principalTable: "Companies",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

                migrationBuilder.CreateTable(
                    name: "Contacts",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false),
                        Phone = table.Column<string>(type: "TEXT", nullable: true),
                        Email = table.Column<string>(type: "TEXT", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Contacts", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Contacts_Persons_Id",
                            column: x => x.Id,
                            principalTable: "Persons",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_Persons_CompanyId",
                    table: "Persons",
                    column: "CompanyId");
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropTable(
                    name: "Contacts");

                migrationBuilder.DropTable(
                    name: "Persons");

                migrationBuilder.DropTable(
                    name: "Companies");
            }
        }

        #endregion

        CrudContext _db;

        [SetUp]
        public void StartUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CrudContext>();
            optionsBuilder.UseSqlite("Filename = Crud.db");

            _db = new CrudContext(optionsBuilder.Options);
        }

        [Test]
        public void Projection()
        {
            // Perform:
            // SELECT "c"."Id", "c"."Email"
            // FROM "Contacts" AS "c"

            _db.Contacts.Select(c => new { c.Id, c.Email })
                .ToList()
                .ForEach(c => Console.WriteLine($"id: {c.Id}, email: {c.Email}"));
        }

        [Test]
        public void NoTraking()
        {
            Contact contact = _db.Contacts.AsNoTracking().Single(c => c.Id == 1);

            Contact newContact = new Contact { Id = contact.Id, Email = contact.Email, Phone = contact.Phone };
            _db.Contacts.Add(newContact);
            _db.SaveChanges(); // It will not trhow an exception
        }

        [Test]
        public void SqlRow()
        {
            List<Contact> contacts = _db.Contacts.FromSqlRaw("Select * From Contacts").ToList();

            int id = 1;
            contacts = _db.Contacts.FromSqlInterpolated($"exec dbo.StoredProcedure {id}").ToList();
        }

        [Test]
        public void SubQuery()
        {
            List<Company> companies = _db.Companies.Include(c => c.Persons.Where(p => p.Id < 2)).ToList();

            companies = _db.Companies.Include(c => c.Persons.OrderBy(p => p.Name).Take(3)).ToList();
        }
    }
}
