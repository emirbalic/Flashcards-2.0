using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocabularyTrainer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExampleSentenceToTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExampleSentence",
                table: "FlashcardTranslations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExampleSentence",
                table: "FlashcardTranslations");
        }
    }
}
