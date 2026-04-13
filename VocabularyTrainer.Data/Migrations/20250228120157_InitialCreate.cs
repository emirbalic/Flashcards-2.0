using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VocabularyTrainer.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    German = table.Column<string>(type: "TEXT", nullable: false),
                    English = table.Column<string>(type: "TEXT", nullable: false),
                    Croatian = table.Column<string>(type: "TEXT", nullable: false),
                    French = table.Column<string>(type: "TEXT", nullable: false),
                    ExampleSentence = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "Id", "Croatian", "English", "ExampleSentence", "French", "German" },
                values: new object[,]
                {
                    { 1, "Kuća", "House", "Das Haus ist groß.", "Maison", "Haus" },
                    { 2, "Drvo", "Tree", "Der Baum ist hoch.", "Arbre", "Baum" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");
        }
    }
}
