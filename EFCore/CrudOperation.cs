using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace EFCoreLaucher
{
    public class CrudOperation
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
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
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

        private void Writes(params Contact[] contacts)
        {
            foreach (Contact contact in contacts)
            {
                Console.WriteLine($"phone {contact.Phone}, mail {contact.Email}");
            }
        }
        private void Writes(params Person[] persons)
        {
            foreach (Person person in persons)
            {
                Console.WriteLine($"name {person.Name}");
            }
        }

        [Test]
        public void Read()
        {
            List<Contact> contactList = _db.Contacts.ToList(); // Gets all entities
            Writes(contactList.ToArray());

            contactList = _db.Contacts.Where(c => c.Id % 2 == 0).ToList(); // Perform a Where query
            Writes(contactList.ToArray());

            Contact contact = _db.Contacts.Find(contactList.First().Id); // Perform a Where query by primary key
            Writes(contact);

            contact = _db.Contacts.SingleOrDefault(c => c.Id == contactList.First().Id); // Perform a Where query, if it return more than one record it throw an Exception
            Writes(contact);
        }

        [Test]
        public void ExplicitLoadingRead()
        {
            List<Contact> contactList = _db.Contacts.Include(c => c.Person).ToList(); // Load also Person entities
            Writes(contactList.ToArray());
            Writes(contactList.Select(c => c.Person).ToArray());

            Contact contact = _db.Contacts.First();
            _db.Entry(contact).Reference(c => c.Person).Load(); // It will load from the Db o from the Memory
            Writes(contact.Person);
        }


        [Test]
        public void Insert()
        {
            Person person = new Person { Name = "test", Company = _db.Companies.First() };

            _db.Add(person);
            _db.SaveChanges();

            List<Person> personList = new List<Person>
            {
                new Person { Name = "test1", Company = _db.Companies.First() },
                new Person { Name = "test2", Company = _db.Companies.First() },
                new Person { Name = "test3", Company = _db.Companies.First() }
            };

            _db.Persons.AddRange(personList);
            _db.SaveChanges();
        }

        [Test]
        public void Update()
        {
            Person person = _db.Persons.First();

            person.Name = "Test";

            _db.Update(person); // In thi case unnecessary
            _db.SaveChanges();
        }

        [Test]
        public void Delete()
        {
            Person person = _db.Persons.First();

            _db.Remove(person);
            _db.SaveChanges();
        }


    }
}
