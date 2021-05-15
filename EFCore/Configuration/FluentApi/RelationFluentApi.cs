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
    public class RelationFluentApi
    {
        #region Model

        public class Book
        {
            public int Id { get; set; } // key
            public DateTime ReleaseDate { get; set; } // Key
            public string Title { get; set; }

            // One to One relationship
            public BookDetail BookDetail { get; set; }

            // One to Many relationship
            public int CategoryId { get; set; } // Optional
            public Category Category { get; set; }

            // Many to Many relationship
            public List<Author> Authors { get; set; }

        }

        public class BookDetail
        {
            public int Id { get; set; } // key
            public int NumberOfPages { get; set; }
            public int NumberOfChapters { get; set; }

            // One to One realtionship
            public int BookId { get; set; } 
            public DateTime BookReleaseDate { get; set; }
            public Book Book { get; set; }
        }


        public class Category
        {
            public int Id { get; set; } // key
            public string Name { get; set; }

            // One to Many relationship
            public List<Book> Books { get; set; }
        }

        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }

            //Many to Many relationship
            public List<Book> Books { get; set; }
        }

        #endregion

        #region Context

        public class RelationFluentApiContextFactory : IDesignTimeDbContextFactory<RelationFluentApiContext>
        {
            public RelationFluentApiContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<RelationFluentApiContext>();
                optionsBuilder.UseSqlite("Filename = RelationFluentApi.db");

                return new RelationFluentApiContext(optionsBuilder.Options);
            }
        }

        public class RelationFluentApiContext : DbContext
        {
            public RelationFluentApiContext(DbContextOptions<RelationFluentApiContext> options) : base() { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = RelationFluentApi.db");
            }

            public DbSet<Book> Books { get; set; }
            public DbSet<BookDetail> BookDetails { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Author> Authors { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configure composite Key
                modelBuilder.Entity<Book>().HasKey(e => new { e.Id, e.ReleaseDate});

                // One to One relation
                modelBuilder.Entity<Book>().HasOne(e => e.BookDetail).WithOne(e => e.Book)
                    .HasForeignKey<BookDetail>(e => new { e.BookId, e.BookReleaseDate });

                // One to Many relation
                modelBuilder.Entity<Book>().HasOne(e => e.Category).WithMany(e => e.Books)
                    .HasForeignKey(e => e.CategoryId);

                // Many to Many relation
                modelBuilder.Entity<Book>().HasMany(e => e.Authors).WithMany(e => e.Books);                    
            }

        }

        #endregion

        #region Migration 

        public partial class RelationFluentApiMigration : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.CreateTable(
                    name: "Authors",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Name = table.Column<string>(type: "TEXT", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Authors", x => x.Id);
                    });

                migrationBuilder.CreateTable(
                    name: "Categories",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Name = table.Column<string>(type: "TEXT", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Categories", x => x.Id);
                    });

                migrationBuilder.CreateTable(
                    name: "Books",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false),
                        ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                        Title = table.Column<string>(type: "TEXT", nullable: true),
                        CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Books", x => new { x.Id, x.ReleaseDate });
                        table.ForeignKey(
                            name: "FK_Books_Categories_CategoryId",
                            column: x => x.CategoryId,
                            principalTable: "Categories",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateTable(
                    name: "AuthorBook",
                    columns: table => new
                    {
                        AuthorsId = table.Column<int>(type: "INTEGER", nullable: false),
                        BooksId = table.Column<int>(type: "INTEGER", nullable: false),
                        BooksReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId, x.BooksReleaseDate });
                        table.ForeignKey(
                            name: "FK_AuthorBook_Authors_AuthorsId",
                            column: x => x.AuthorsId,
                            principalTable: "Authors",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_AuthorBook_Books_BooksId_BooksReleaseDate",
                            columns: x => new { x.BooksId, x.BooksReleaseDate },
                            principalTable: "Books",
                            principalColumns: new[] { "Id", "ReleaseDate" },
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateTable(
                    name: "BookDetails",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        NumberOfPages = table.Column<int>(type: "INTEGER", nullable: false),
                        NumberOfChapters = table.Column<int>(type: "INTEGER", nullable: false),
                        BookId = table.Column<int>(type: "INTEGER", nullable: false),
                        BookReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_BookDetails", x => x.Id);
                        table.ForeignKey(
                            name: "FK_BookDetails_Books_BookId_BookReleaseDate",
                            columns: x => new { x.BookId, x.BookReleaseDate },
                            principalTable: "Books",
                            principalColumns: new[] { "Id", "ReleaseDate" },
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_AuthorBook_BooksId_BooksReleaseDate",
                    table: "AuthorBook",
                    columns: new[] { "BooksId", "BooksReleaseDate" });

                migrationBuilder.CreateIndex(
                    name: "IX_BookDetails_BookId_BookReleaseDate",
                    table: "BookDetails",
                    columns: new[] { "BookId", "BookReleaseDate" },
                    unique: true);

                migrationBuilder.CreateIndex(
                    name: "IX_Books_CategoryId",
                    table: "Books",
                    column: "CategoryId");
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropTable(
                    name: "AuthorBook");

                migrationBuilder.DropTable(
                    name: "BookDetails");

                migrationBuilder.DropTable(
                    name: "Authors");

                migrationBuilder.DropTable(
                    name: "Books");

                migrationBuilder.DropTable(
                    name: "Categories");
            }
        }

        #endregion
    }
}
