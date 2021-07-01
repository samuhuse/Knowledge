using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    public class AutoMapperExtentions
    {
        #region Model
        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
        }

        public class PersonDto
        {
            public string Name { get; set; }
        }

        #endregion

        #region Context

        public class AutoMapperContext : DbContext
        {
            public AutoMapperContext(DbContextOptions<AutoMapperContext> options) : base(options) { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = AutoMapperContext.db");
                optionsBuilder.LogTo(Console.WriteLine);
            }

            public DbSet<Person> Persons { get; set; }
        }

        #endregion

        #region Migration

        public partial class CrudContextMigration : Migration
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
                        Contact = table.Column<int>(type: "TEXT", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Persons", x => x.Id);                       
                    });
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropTable(
                    name: "Persons");
            }
        }

        #endregion

        #region AutoMapper configuration

        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                CreateMap<Person, PersonDto>();
            }
        }

        #endregion

        AutoMapperContext _db;
        IMapper _mapper;

        [SetUp]
        public void StartUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AutoMapperContext>();
            optionsBuilder.UseSqlite("Filename = Crud.db");

            _db = new AutoMapperContext(optionsBuilder.Options);

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Test]
        public void TryProject()
        {
            IEnumerable<PersonDto> personDtos = _db.Persons.ProjectTo<PersonDto>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
