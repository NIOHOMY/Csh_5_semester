using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) { }

        //{
        //    migrationBuilder.CreateTable(
        //        name: "Authors",
        //        columns: table => new
        //        {
        //            AuthorId = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Authors", x => x.AuthorId);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Publishers",
        //        columns: table => new
        //        {
        //            PublisherId = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            NameOfPublisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            City = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Publishers", x => x.PublisherId);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Readers",
        //        columns: table => new
        //        {
        //            ReaderId = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Readers", x => x.ReaderId);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Books",
        //        columns: table => new
        //        {
        //            BookId = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            FirstAuthorId = table.Column<int>(type: "int", nullable: false),
        //            YearOfPublication = table.Column<int>(type: "int", nullable: false),
        //            Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
        //            NumberOfExamples = table.Column<int>(type: "int", nullable: false),
        //            PublisherId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Books", x => x.BookId);
        //            table.ForeignKey(
        //                name: "FK_Books_Authors_FirstAuthorId",
        //                column: x => x.FirstAuthorId,
        //                principalTable: "Authors",
        //                principalColumn: "AuthorId",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_Books_Publishers_PublisherId",
        //                column: x => x.PublisherId,
        //                principalTable: "Publishers",
        //                principalColumn: "PublisherId",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Issues",
        //        columns: table => new
        //        {
        //            IssueId = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            ReaderId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Issues", x => x.IssueId);
        //            table.ForeignKey(
        //                name: "FK_Issues_Readers_ReaderId",
        //                column: x => x.ReaderId,
        //                principalTable: "Readers",
        //                principalColumn: "ReaderId",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "IssueBooks",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            BookId = table.Column<int>(type: "int", nullable: false),
        //            IssueId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_IssueBooks", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_IssueBooks_Books_BookId",
        //                column: x => x.BookId,
        //                principalTable: "Books",
        //                principalColumn: "BookId",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_IssueBooks_Issues_IssueId",
        //                column: x => x.IssueId,
        //                principalTable: "Issues",
        //                principalColumn: "IssueId",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Books_FirstAuthorId",
        //        table: "Books",
        //        column: "FirstAuthorId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Books_PublisherId",
        //        table: "Books",
        //        column: "PublisherId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_IssueBooks_BookId",
        //        table: "IssueBooks",
        //        column: "BookId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_IssueBooks_IssueId",
        //        table: "IssueBooks",
        //        column: "IssueId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Issues_ReaderId",
        //        table: "Issues",
        //        column: "ReaderId");
        //}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) { }
        /*{
            migrationBuilder.DropTable(
                name: "IssueBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Readers");
        }*/
    }
}
