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
    public class RelationDataAnnotation
    {
        #region Model

        public class Book
        {
            [Key]
            public int Id { get; set; }
            public string Title { get; set; }

            // One to One relationship
            public BookDetail BookDetail { get; set; }

            // One to Many relationship
            [ForeignKey("Category")]
            public int CategoryId { get; set; } // Optional
            public Category Category { get; set; }

            // Many to Many relationship
            public List<Author> Authors { get; set; }

        }

        public class BookDetail
        {
            public int Id { get; set; }
            public int NumberOfPages { get; set; }
            public int NumberOfChapters { get; set; }

            // One to One realtionship
            [ForeignKey("Book")]
            public int BookId { get; set; } // Optional
            public Book Book { get; set; }
        }


        public class Category
        {
            [Key]
            public int Id { get; set; }
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

        public class RelationDataAnnotationContextFactory : IDesignTimeDbContextFactory<RelationDataAnnotationContext>
        {
            public RelationDataAnnotationContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<RelationDataAnnotationContext>();
                optionsBuilder.UseSqlite("Filename = RelationDataAnnotation.db");

                return new RelationDataAnnotationContext(optionsBuilder.Options);
            }
        }

        public class RelationDataAnnotationContext : DbContext
        {
            public RelationDataAnnotationContext(DbContextOptions<RelationDataAnnotationContext> options) : base() { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Filename = RelationDataAnnotation.db");
            }

            public DbSet<Book> Books { get; set; }
            public DbSet<BookDetail> BookDetails { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Author> Authors { get; set; }

        }

        #endregion

        #region Migration

        public partial class RelationDataAnnotationMigration : Migration
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
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Title = table.Column<string>(type: "TEXT", nullable: true),
                        CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Books", x => x.Id);
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
                        BooksId = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                        table.ForeignKey(
                            name: "FK_AuthorBook_Authors_AuthorsId",
                            column: x => x.AuthorsId,
                            principalTable: "Authors",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_AuthorBook_Books_BooksId",
                            column: x => x.BooksId,
                            principalTable: "Books",
                            principalColumn: "Id",
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
                        BookId = table.Column<int>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_BookDetails", x => x.Id);
                        table.ForeignKey(
                            name: "FK_BookDetails_Books_BookId",
                            column: x => x.BookId,
                            principalTable: "Books",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_AuthorBook_BooksId",
                    table: "AuthorBook",
                    column: "BooksId");

                migrationBuilder.CreateIndex(
                    name: "IX_BookDetails_BookId",
                    table: "BookDetails",
                    column: "BookId",
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
